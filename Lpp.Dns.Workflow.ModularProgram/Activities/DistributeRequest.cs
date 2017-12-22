using Lpp.Utilities;
using Lpp.Dns.Data;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO.Security;
using System.Xml;
using Lpp.Utilities.Logging;

namespace Lpp.Dns.Workflow.ModularProgram.Activities
{
    public class DistributeRequest : ActivityBase<Request>
    {
        static readonly Guid TerminateResultID = new Guid("53579F36-9D20-47D9-AC33-643D9130080B");
        static readonly Guid SubmitResultID = new Guid("5445DC6E-72DC-4A6B-95B6-338F0359F89E");
        static readonly Guid ModifyWorkingSpecificationsResultID = new Guid("E970F48E-A072-41C1-B8C3-CC34C5826A46");
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
       
        public override Guid ID
        {
            get { return ModularProgramWorkflowConfiguration.DistributeRequestID; }
        }

        public override string Uri
        {
            get { return "/requests/details?ID=" + this._workflow.Entity.ID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Distribution";
            }
        }

        public override string CustomTaskSubject
        {
            get
            {
                return "Distribute Request";
            }
        }

        public override async Task<Lpp.Workflow.Engine.ValidationResult> Validate(Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultIDRequired };
            }

            if (activityResultID.Value != TerminateResultID && 
                activityResultID.Value != SubmitResultID && 
                activityResultID.Value != ModifyWorkingSpecificationsResultID &&
                activityResultID.Value != SaveResultID)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultNotSupported };
            }

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow);

            if (activityResultID.Value != TerminateResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToCloseTask };
            }
            else if(activityResultID.Value == TerminateResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToTerminateWorkflow };
            }

            string errors;
            if (activityResultID.Value == SubmitResultID)
            {
                errors = await PerformSubmitValidation();
            }
            else
            {
                errors = string.Empty;
            }


            if (errors.Length > 0)
            {
                return new ValidationResult
                {
                    Success = false,
                    Errors = errors.ToString()
                };
            }
            else
            {
                return new ValidationResult
                {
                    Success = true
                };
            }
        }

        public override async Task<CompletionResult> Complete(string data, Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
            {
                throw new ArgumentNullException(CommonMessages.ActivityResultIDRequired);
            }

            if (activityResultID.Value != TerminateResultID &&
                activityResultID.Value != SubmitResultID && 
                activityResultID.Value != ModifyWorkingSpecificationsResultID && 
                activityResultID.Value != SaveResultID)
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }

            var task = await PmnTask.GetActiveTaskForRequestActivityAsync(_entity.ID, ID, db);
            await db.Entry(_entity).ReloadAsync();

            if (activityResultID.Value == TerminateResultID)
            {
                _entity.CancelledByID = _workflow.Identity.ID;
                _entity.CancelledOn = DateTime.UtcNow;

                task.Status = DTO.Enums.TaskStatuses.Cancelled;
                await db.SaveChangesAsync();

                return new CompletionResult
                {
                    ResultID = TerminateResultID
                };
            }
            
            
            if (activityResultID.Value == SubmitResultID)
            {
                await db.LoadCollection(_entity, (r) => r.DataMarts);

                if (!_entity.DataMarts.Any())
                    throw new Exception("At least one routing needs to be specified when submitting a requests.");

                //prepare the request documents, save created documents same as legacy
                IList<Guid> documentRevisionSets = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Guid>>(data);

                IEnumerable<Document> documents = await (from d in db.Documents.AsNoTracking()
                                                         join x in (
                                                             db.Documents.Where(dd => documentRevisionSets.Contains(dd.RevisionSetID.Value))
                                                             .GroupBy(k => k.RevisionSetID)
                                                             .Select(k => k.OrderByDescending(d => d.MajorVersion).ThenByDescending(d => d.MinorVersion).ThenByDescending(d => d.BuildVersion).ThenByDescending(d => d.RevisionVersion).Select(y => y.ID).Distinct().FirstOrDefault())
                                                         ) on d.ID equals x
                                                         orderby d.ItemID descending, d.RevisionSetID descending, d.CreatedOn descending
                                                         select d).ToArrayAsync();

                await db.Entry(_entity).Reference(r => r.Activity).LoadAsync();
                await db.Entry(_entity).Reference(r => r.RequestType).LoadAsync();
                string submitterEmail = await db.Users.Where(u => u.ID == _workflow.Identity.ID).Select(u => u.Email).SingleAsync();

                string xmlContent = SerializeModularProgramsToXML(documents, submitterEmail);
                string htmlContent = SerializeModularProgramsToHTML(documents, submitterEmail);

                Document existingDocument = await (from d in db.Documents.AsNoTracking()
                                                   join x in (
                                                       db.Documents.Where(dd => dd.ItemID == task.ID && dd.FileName == "ModularProgramRequest.xml")
                                                       .GroupBy(k => k.RevisionSetID)
                                                       .Select(k => k.OrderByDescending(d => d.MajorVersion).ThenByDescending(d => d.MinorVersion).ThenByDescending(d => d.BuildVersion).ThenByDescending(d => d.RevisionVersion).Select(y => y.ID).Distinct().FirstOrDefault())
                                                   ) on d.ID equals x
                                                   orderby d.ItemID descending, d.RevisionSetID descending, d.CreatedOn descending
                                                   select d).FirstOrDefaultAsync();

                Document xmlRequestDoc = db.Documents.Add(new Document
                {
                    Name = "ModularProgramRequest.xml",
                    FileName = "ModularProgramRequest.xml",
                    ItemID = task.ID,
                    Kind = DTO.Enums.DocumentKind.SystemGeneratedNoLog,
                    MimeType = "application/xml",
                    Viewable = false,
                    UploadedByID = _workflow.Identity.ID,
                    Length = System.Text.UTF8Encoding.UTF8.GetByteCount(xmlContent)
                });

                db.ActionReferences.Add(new TaskReference { TaskID = task.ID, ItemID = xmlRequestDoc.ID, Type = DTO.Enums.TaskItemTypes.ActivityDataDocument });

                if (existingDocument == null)
                {
                    xmlRequestDoc.RevisionSetID = xmlRequestDoc.ID;
                }
                else
                {
                    xmlRequestDoc.ParentDocumentID = existingDocument.ID;
                    xmlRequestDoc.Name = existingDocument.Name;
                    xmlRequestDoc.Description = existingDocument.Description;
                    xmlRequestDoc.RevisionSetID = existingDocument.RevisionSetID;
                    xmlRequestDoc.MajorVersion = existingDocument.MajorVersion;
                    xmlRequestDoc.MinorVersion = existingDocument.MinorVersion;
                    xmlRequestDoc.BuildVersion = existingDocument.BuildVersion;
                    xmlRequestDoc.RevisionVersion = existingDocument.RevisionVersion + 1;
                }

                existingDocument = await (from d in db.Documents.AsNoTracking()
                                          join x in (
                                              db.Documents.Where(dd => dd.ItemID == task.ID && dd.FileName == "ModularProgramRequest.html")
                                              .GroupBy(k => k.RevisionSetID)
                                              .Select(k => k.OrderByDescending(d => d.MajorVersion).ThenByDescending(d => d.MinorVersion).ThenByDescending(d => d.BuildVersion).ThenByDescending(d => d.RevisionVersion).Select(y => y.ID).Distinct().FirstOrDefault())
                                          ) on d.ID equals x
                                          orderby d.ItemID descending, d.RevisionSetID descending, d.CreatedOn descending
                                          select d).FirstOrDefaultAsync();

                Document htmlRequestDoc = db.Documents.Add(new Document
                {
                    Name = "ModularProgramRequest.html",
                    FileName = "ModularProgramRequest.html",
                    ItemID = task.ID,
                    Kind = DTO.Enums.DocumentKind.SystemGeneratedNoLog,
                    MimeType = "text/html",
                    Viewable = true,
                    UploadedByID = _workflow.Identity.ID,
                    Length = System.Text.UTF8Encoding.UTF8.GetByteCount(htmlContent)
                });

                db.ActionReferences.Add(new TaskReference { TaskID = task.ID, ItemID = htmlRequestDoc.ID, Type = DTO.Enums.TaskItemTypes.ActivityDataDocument });

                if (existingDocument == null)
                {
                    htmlRequestDoc.RevisionSetID = htmlRequestDoc.ID;
                }
                else
                {
                    htmlRequestDoc.ParentDocumentID = existingDocument.ID;
                    htmlRequestDoc.Name = existingDocument.Name;
                    htmlRequestDoc.Description = existingDocument.Description;
                    htmlRequestDoc.RevisionSetID = existingDocument.RevisionSetID;
                    htmlRequestDoc.MajorVersion = existingDocument.MajorVersion;
                    htmlRequestDoc.MinorVersion = existingDocument.MinorVersion;
                    htmlRequestDoc.BuildVersion = existingDocument.BuildVersion;
                    htmlRequestDoc.RevisionVersion = existingDocument.RevisionVersion + 1;
                }

                //save the document metadata
                await db.SaveChangesAsync();

                //push the document bytes
                xmlRequestDoc.SetData(db, System.Text.Encoding.UTF8.GetBytes(xmlContent));
                htmlRequestDoc.SetData(db, System.Text.Encoding.UTF8.GetBytes(htmlContent));

                //add the system generated documents revisionsetIDs to the collection of request documents to be submitted to the data partners.
                documentRevisionSets.Add(xmlRequestDoc.RevisionSetID.Value);
                documentRevisionSets.Add(htmlRequestDoc.RevisionSetID.Value);

                //update the request
                _entity.SubmittedByID = _workflow.Identity.ID;
                _entity.SubmittedOn = DateTime.UtcNow;
                _entity.AdapterPackageVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(this.GetType().Assembly.Location).FileVersion;
                _entity.RejectedByID = null;
                _entity.RejectedOn = null;
                _entity.Private = false;               

                //save the changes to the request now since the trigger for routings will change the status invalidating the object before save
                await db.SaveChangesAsync();

                var originalStatus = _entity.Status;
                await SetRequestStatus(DTO.Enums.RequestStatuses.Submitted, false);

                foreach (var dm in _entity.DataMarts.Where(dm => dm.Status == 0 || dm.Status == DTO.Enums.RoutingStatus.AwaitingRequestApproval || dm.Status == DTO.Enums.RoutingStatus.Draft))
                {
                    dm.Status = DTO.Enums.RoutingStatus.Submitted;

                    var currentResponse = db.Responses.FirstOrDefault(r => r.RequestDataMartID == dm.ID && r.Count == r.RequestDataMart.Responses.Max(rr => rr.Count));
                    if (currentResponse == null)
                    {
                        currentResponse = db.Responses.Add(new Response { RequestDataMartID = dm.ID });
                    }
                    currentResponse.SubmittedByID = _workflow.Identity.ID;
                    currentResponse.SubmittedOn = DateTime.UtcNow;

                    //add the request document associations
                    for (int i = 0; i < documentRevisionSets.Count; i++)
                    {
                        db.RequestDocuments.Add(new RequestDocument { RevisionSetID = documentRevisionSets[i], ResponseID = currentResponse.ID, DocumentType = DTO.Enums.RequestDocumentType.Input });
                    }
                }

                await db.SaveChangesAsync();
                //reload the request since altering the routings triggers a change of the request status in the db by a trigger.
                await db.Entry(_entity).ReloadAsync();

                DTO.QueryComposer.QueryComposerRequestDTO qcRequestDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(_entity.Query);
                var modularTerm = qcRequestDTO.Where.Criteria.SelectMany(c => c.Terms.Where(t => t.Type == SimpleModularProgramWorkflowConfiguration.ModularProgramTermID)).FirstOrDefault();
                var termValues = Newtonsoft.Json.JsonConvert.DeserializeObject<ModularProgramTermValues>(modularTerm.Values["Values"].ToString());

                //update the request.json term value to include system generated documents revisionsetIDs
                termValues.Documents.Clear();

                for (int i = 0; i < documentRevisionSets.Count; i++)
                {
                    termValues.Documents.Add(new ModularProgramTermValues.Document { RevisionSetID = documentRevisionSets[i] });
                }

                modularTerm.Values["Values"] = termValues;
                _entity.Query = Newtonsoft.Json.JsonConvert.SerializeObject(qcRequestDTO);

                await db.SaveChangesAsync();

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.Submitted);               

            }

            if (activityResultID.Value == ModifyWorkingSpecificationsResultID)
            {
                var originalStatus = _entity.Status;
                await SetRequestStatus(DTO.Enums.RequestStatuses.PendingWorkingSpecification,false);

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.PendingWorkingSpecification);
            }

            await MarkTaskComplete(task);

            return new CompletionResult
            {
                ResultID = activityResultID.Value
            };
        }

        public override async Task Start(string comment)
        {
            await base.Start(comment);

            if (!string.IsNullOrEmpty(_entity.Query) && !_entity.Query.StartsWith("{\"Header\"", StringComparison.OrdinalIgnoreCase))
            {
                //make sure the query field is empty
                _entity.Query = null;
                await db.SaveChangesAsync();
            }
        }

        string SerializeModularProgramsToXML(IEnumerable<Document> documents, string submitterEmail)
        {
            StringBuilder sb = new StringBuilder();
            System.Xml.XmlWriterSettings settings = new XmlWriterSettings();
            settings.CloseOutput = true;
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.OmitXmlDeclaration = true;
            settings.WriteEndDocumentOnClose = true;

            using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sb, settings))
            {
                writer.WriteStartElement("request_builder");
                writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");

                writer.WriteStartElement("header");
                writer.WriteElementString("request_type", _entity.RequestType != null ? _entity.RequestType.Name : string.Empty);
                writer.WriteElementString("request_name", _entity.Name);
                if (!string.IsNullOrWhiteSpace(_entity.Description))
                    writer.WriteElementString("request_description", _entity.Description);
                if (_entity.DueDate.HasValue)
                    writer.WriteElementString("due_date", _entity.DueDate.Value.ToShortDateString());
                if (_entity.Activity != null)
                    writer.WriteElementString("activity", _entity.Activity.Name);
                if (_entity.Activity != null && !string.IsNullOrWhiteSpace(_entity.Activity.Description))
                    writer.WriteElementString("activity_description", _entity.ActivityDescription);
                writer.WriteElementString("submitter_email", submitterEmail);
                writer.WriteEndElement();//close header

                writer.WriteStartElement("request");
                writer.WriteElementString("PackageManifest", string.Empty);
                writer.WriteStartElement("Files");

                foreach (var d in documents)
                {
                    writer.WriteStartElement("File");
                    writer.WriteElementString("Name", d.FileName);
                    writer.WriteElementString("MimeType", d.MimeType);
                    writer.WriteElementString("Size", d.Length.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();//close Files
                writer.WriteEndElement();//close request
            }

            return sb.ToString();
        }

        string SerializeModularProgramsToHTML(IEnumerable<Document> documents, string submitterEmail)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var doc in documents)
            {
                sb.AppendLine("                <tr><td>" + System.Net.WebUtility.HtmlEncode(doc.FileName) + "</td><td>" + System.Net.WebUtility.HtmlEncode(doc.MimeType) + "</td><td>" + doc.Length.ToString() + "</td></tr>");
            }

            return string.Format(HtmlTemplate, _entity.Name, _entity.RequestType != null ? _entity.RequestType.Name : "", submitterEmail, sb.ToString().TrimEnd());
        }

        const string HtmlTemplate = @"<html>
          <body style=""font-family: Calibri; font-size: font-size: 16px;"">
            <table style=""border-style:solid; border-width: 2px; width: 620px; margin:4px; padding:4px"">
              <tr>
                <td>
                  <table style=""width: 600px;  text-align:left"">
                    <tr>
                      <td><b>Request Name: </b>{0}</td>
                      <td><b>Request Type: </b>{1}</td>
                    </tr>
                    <tr>
                      <td colspan=""2""><b>Request Submitter: </b>{2}</td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr>
                <td>
                  <hr>
                </td>
              </tr>
              <tr>
                <td><b>Files</b>
                  <table style=""border-style:solid; border-width: 1px; border-color: Gray; width: 600px; text-align:left;"">
                    <thead style=""background-color: #E3E6EB; font-weight: bold; "">
                      <th>Name</th>
                      <th width=""100px"">Mime Type</th>
                      <th width=""100px"">Size</th>
                    </thead>
                    <tbody>
        {3}
                    </tbody>
                  </table>
                </td>
              </tr>
            </table>
          </body>
        </html>";

    }
}
