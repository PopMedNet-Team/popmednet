using Lpp.Dns.Data;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Lpp.Utilities.Logging;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Workflow.SummaryQuery.Activities
{
    public class DistributeRequest : ActivityBase<Request>
    {
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        static readonly Guid SubmitResultID = new Guid("80FD6F76-2E32-4D35-9797-0B541507CB56");
        static readonly Guid ModifyResultID = new Guid("94513F48-4C4A-4449-BA95-5B0CD81DB642");
        static readonly Guid TerminateResultID = new Guid("53579F36-9D20-47D9-AC33-643D9130080B");

        private const string DocumentKind = "Lpp.Dns.Workflow.SummaryQuery.Activities.Request";

        public override Guid ID
        {
            get { return SummaryQueryWorkflowConfiguration.DistributeRequestID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Distribution";
            }
        }

        public override string Uri
        {
            get { return "requests/details?ID=" + _entity.ID; }
        }
        public override string CustomTaskSubject
        {
            get
            {
                return "Distribute Request";
            }
        }

        public override async Task<ValidationResult> Validate(Guid? activityResultID)
        {
            if (activityResultID == null)
            {
                return new ValidationResult
                {
                    Success = false,
                    Errors = CommonMessages.ActivityResultIDRequired
                };
            }

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity);
            
            if (!permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask) && (activityResultID.Value == SaveResultID || activityResultID.Value == ModifyResultID))
            {
                return new ValidationResult
                {
                    Success = false,
                    Errors = CommonMessages.RequirePermissionToEditTask
                };
            }

            if (!permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask) && (activityResultID.Value == SubmitResultID))
            {
                return new ValidationResult
                {
                    Success = false,
                    Errors = CommonMessages.RequirePermissionToCloseTask
                };
            }

            if (!permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask) && (activityResultID.Value == TerminateResultID))
            {
                return new ValidationResult
                {
                    Success = false,
                    Errors = CommonMessages.RequirePermissionToTerminateWorkflow
                };
            }

            var errors = new StringBuilder();
            if (activityResultID.Value == SubmitResultID)
            {
                if (_entity.ProjectID == null)
                    errors.AppendHtmlLine("Please ensure that you have selected a project for the request.");

                if (_entity.DueDate.HasValue && _entity.DueDate.Value < DateTime.UtcNow)
                    errors.AppendHtmlLine("The Request Due Date must be set in the future.");

                var dataMartsDueDate = false;
                foreach (var dm in _entity.DataMarts)
                {
                    if (dm.DueDate.HasValue && dm.DueDate.Value < DateTime.UtcNow)
                        dataMartsDueDate = true;
                }
                if (dataMartsDueDate)
                    errors.AppendHtmlLine("The Request's DataMart Due Dates must be set in the future.");

                if (_entity.SubmittedOn.HasValue)
                    errors.AppendHtmlLine("Cannot submit a request that has already been submitted");

                if (_entity.Template)
                    errors.AppendHtmlLine("Cannot submit a request template");

                if (_entity.Scheduled)
                    errors.AppendHtmlLine("Cannot submit a scheduled request");

                await db.LoadReference(_entity, (r) => r.Project);

                //If a project loaded successfully check it.
                if (_entity.Project != null)
                {
                    if (!_entity.Project.Active || _entity.Project.Deleted)
                        errors.AppendHtmlLine("Cannot submit a request for an inactive or deleted project.");

                    if (_entity.Project.EndDate < DateTime.UtcNow)
                        errors.AppendHtmlLine("Cannot submit a request for a project that has ended.");

                    await db.LoadCollection(_entity.Project, (p) => p.DataMarts);

                    if (_entity.DataMarts.Any(dm => !_entity.Project.DataMarts.Any(pdm => pdm.DataMartID == dm.DataMartID)))
                        errors.AppendHtmlLine("The request contains datamarts that are not part of the project specified and thus cannot be processed. Please remove these datamarts and try again.");
                }

                await db.LoadCollection(_entity, (r) => r.DataMarts);
                var dataMarts = _entity.GetGrantedDataMarts(db, _workflow.Identity);

                if (_entity.DataMarts.Any(dm => !dataMarts.Any(gdm => gdm.ID == dm.DataMartID)))
                    errors.AppendHtmlLine("This request contains datamarts you are not permitted to submit to. Please remove them and try again.");


                var filters = new ExtendedQuery
                {
                    Projects = (a) => a.ProjectID == _entity.ProjectID,
                    ProjectOrganizations = a => a.ProjectID == _entity.ProjectID && a.OrganizationID == _entity.OrganizationID,
                    Organizations = a => a.OrganizationID == _entity.OrganizationID,
                    Users = a => a.UserID == _entity.CreatedByID
                };

                if (_entity.DataMarts.Count < 2)
                {
                    var skip2DataMartRulePerms = await db.HasGrantedPermissions<Request>(_workflow.Identity, _entity, filters, PermissionIdentifiers.Portal.SkipTwoDataMartRule);

                    if (!skip2DataMartRulePerms.Contains(PermissionIdentifiers.Portal.SkipTwoDataMartRule))
                        errors.AppendHtmlLine("Cannot submit a request with less than 2 datamarts");
                }
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
                throw new ArgumentNullException(CommonMessages.ActivityResultIDRequired);

            var task = PmnTask.GetActiveTaskForRequestActivity(_entity.ID, ID, db);
            
            if (activityResultID.Value == SaveResultID)
            {
                if (Newtonsoft.Json.JsonConvert.DeserializeObject<Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO>(_entity.Query).Where.Criteria.Any(c => c.Terms.Any(t => t.Type.ToString().ToUpper() == "2F60504D-9B2F-4DB1-A961-6390117D3CAC") || c.Criteria.Any(ic => ic.Terms.Any(t => t.Type.ToString().ToUpper() == "2F60504D-9B2F-4DB1-A961-6390117D3CAC"))))
                {
                    await db.Entry(_entity).ReloadAsync();
                    _entity.Private = false;
                    _entity.SubmittedByID = _workflow.Identity.ID;

                    //Reset reject for resubmit.
                    _entity.RejectedByID = null;
                    _entity.RejectedOn = null;

                    var originalStatus = _entity.Status;
                    await SetRequestStatus(DTO.Enums.RequestStatuses.DraftReview);

                    await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.DraftReview);

                    await MarkTaskComplete(task);
                }
                else
                {
                    await task.LogAsModifiedAsync(_workflow.Identity, db);
                    await db.SaveChangesAsync();
                }

                return new CompletionResult
                {
                    ResultID = SaveResultID
                };
            }
            else if (activityResultID.Value == SubmitResultID)
            {
                if (Newtonsoft.Json.JsonConvert.DeserializeObject<Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO>(_entity.Query).Where.Criteria.Any(c => c.Terms.Any(t => t.Type.ToString().ToUpper() == "2F60504D-9B2F-4DB1-A961-6390117D3CAC") || c.Criteria.Any(ic => ic.Terms.Any(t => t.Type.ToString().ToUpper() == "2F60504D-9B2F-4DB1-A961-6390117D3CAC"))))
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

                    //update the request
                    _entity.SubmittedByID = _workflow.Identity.ID;
                    _entity.SubmittedOn = DateTime.UtcNow;
                    _entity.AdapterPackageVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(this.GetType().Assembly.Location).FileVersion;
                    _entity.RejectedByID = null;
                    _entity.RejectedOn = null;
                    _entity.Private = false;


                    //save the changes to the request now since the trigger for routings will change the status invalidating the object before save
                    await db.SaveChangesAsync();
                    await db.Entry(_entity).ReloadAsync();

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
                    var fileUploadTerm = qcRequestDTO.Where.Criteria.SelectMany(c => c.Terms.Where(t => t.Type == FileUploadTermID)).FirstOrDefault();
                    var termValues = Newtonsoft.Json.JsonConvert.DeserializeObject<FileUploadValues>(fileUploadTerm.Values["Values"].ToString());

                    //update the request.json term value to include system generated documents revisionsetIDs
                    termValues.Documents.Clear();

                    for (int i = 0; i < documentRevisionSets.Count; i++)
                    {
                        termValues.Documents.Add(new FileUploadValues.Document { RevisionSetID = documentRevisionSets[i] });
                    }

                    fileUploadTerm.Values["Values"] = termValues;
                    _entity.Query = Newtonsoft.Json.JsonConvert.SerializeObject(qcRequestDTO);

                    await db.SaveChangesAsync();

                    await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.Submitted);


                }
                else
                {
                    //This forces a reload because of a trigger issue that results in the timestamp not being updated.
                    await db.Entry(_entity).ReloadAsync();
                    await db.LoadCollection(_entity, (r) => r.DataMarts);

                    var parentDocument = db.Documents.FirstOrDefault(d => d.ItemID == _entity.ID && d.Kind == DocumentKind && d.ParentDocumentID == null);

                    byte[] documentContent = System.Text.UTF8Encoding.UTF8.GetBytes(_entity.Query ?? string.Empty);
                    var document = new Document
                    {
                        Name = "Request Criteria",
                        MajorVersion = parentDocument == null ? 1 : parentDocument.MajorVersion,
                        MinorVersion = parentDocument == null ? 0 : parentDocument.MinorVersion,
                        RevisionVersion = parentDocument == null ? 0 : parentDocument.RevisionVersion,
                        MimeType = "application/json",
                        Viewable = false,
                        UploadedByID = _workflow.Identity.ID,
                        FileName = "request.json",
                        CreatedOn = DateTime.UtcNow,
                        BuildVersion = parentDocument == null ? 0 : parentDocument.BuildVersion,
                        ParentDocumentID = parentDocument == null ? (Guid?)null : parentDocument.ID,
                        ItemID = task.ID,
                        Length = documentContent.LongLength,
                        Kind = Dns.DTO.Enums.DocumentKind.Request
                    };

                    db.Documents.Add(document);
                    document.RevisionSetID = document.ID;
                    await db.SaveChangesAsync();

                    document.SetData(db, documentContent);

                    _entity.SubmittedByID = _workflow.Identity.ID;
                    _entity.SubmittedOn = DateTime.UtcNow;
                    _entity.AdapterPackageVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(this.GetType().Assembly.Location).FileVersion;
                    _entity.Private = false;
                    _entity.RejectedByID = null;
                    _entity.RejectedOn = null;

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
                        db.RequestDocuments.Add(new RequestDocument { RevisionSetID = document.RevisionSetID.Value, ResponseID = currentResponse.ID, DocumentType = DTO.Enums.RequestDocumentType.Input });
                    }

                    await db.SaveChangesAsync();
                    //reload the request since altering the routings triggers a change of the request status in the db by a trigger.
                    await db.Entry(_entity).ReloadAsync();

                    await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.Submitted);

                    await MarkTaskComplete(task);
                }

                return new CompletionResult
                {
                    ResultID = SubmitResultID
                };
            }
            else if (activityResultID.Value == ModifyResultID)
            {
                //moves back to Request Review
                var originalStatus = _entity.Status;
                await SetRequestStatus(DTO.Enums.RequestStatuses.DraftReview);

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.DraftReview);

                await MarkTaskComplete(task);

                return new CompletionResult
                {
                    ResultID = ModifyResultID
                };
            }
            else if (activityResultID.Value == TerminateResultID)
            {
                _entity.CancelledByID = _workflow.Identity.ID;
                _entity.CancelledOn = DateTime.UtcNow;

                if (task != null)
                {
                    task.Status = DTO.Enums.TaskStatuses.Cancelled;
                }

                await db.SaveChangesAsync();

                return null;
            }
            else
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }
        }
        
        public static readonly Guid FileUploadTermID = new Guid("2F60504D-9B2F-4DB1-A961-6390117D3CAC");
        internal class FileUploadValues
        {
            public IList<Document> Documents { get; set; }

            public class Document
            {
                public Guid RevisionSetID { get; set; }
            }
        }
    }
}
