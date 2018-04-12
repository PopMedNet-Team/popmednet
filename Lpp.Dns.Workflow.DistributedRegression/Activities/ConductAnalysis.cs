using Lpp.Dns.Data;
using Lpp.Dns.DTO.Security;
using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Workflow.DistributedRegression.Activities
{
    /// <summary>
    /// The Activity where all the Analyis from the Analysis Ceneter will view the Response data.  This step will be Completed when a signal file is uploaded to PMN.
    /// </summary>
    public class ConductAnalysis : ActivityBase<Request>
    {
        /// <summary>
        /// The Result ID passed by the Analysis Ceneter to indicate to Terminate the Request.  This closes the Current Task and Sets the Request Status to Cancelled.
        /// </summary>
        private static readonly Guid TerminateResultID = new Guid("53579F36-9D20-47D9-AC33-643D9130080B");
        /// <summary>
        /// The Result ID passed by the Analysis Ceneter to indicate the Request can be saved.  This does not alter the Current Task or Request Status.
        /// </summary>
        private static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

        /// <summary>
        /// Gets the current Activity Name
        /// </summary>
        public override string ActivityName
        {
            get
            {
                return "Complete Analysis";
            }
        }

        /// <summary>
        /// The ID of the Activity
        /// </summary>
        public override Guid ID
        {
            get
            {
                return new Guid("370646FC-7A47-43B5-A4B3-659F90A188A9");
            }
        }

        /// <summary>
        /// The String that shows in the Task Subject Window
        /// </summary>
        public override string CustomTaskSubject
        {
            get
            {
                return "Conduct Analysis";
            }
        }

        /// <summary>
        /// The URL that should be passed back to the User
        /// </summary>
        public override string Uri
        {
            get
            {
                return "requests/details?ID=" + _entity.ID;
            }
        }

        /// <summary>
        /// The Method to Validate the User Permissions and to Validate the Request Information before being passed to the Complete Method
        /// </summary>
        /// <param name="activityResultID">The Result ID passed by the User to indicate which step to proceed to.</param>
        /// <returns></returns>
        public override async Task<ValidationResult> Validate(Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
                activityResultID = SaveResultID;

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask);

            if (!permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask) && (activityResultID.Value == SaveResultID || activityResultID.Value == TerminateResultID))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToEditTask };
            }

            if (!permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask) && (activityResultID.Value == SaveResultID || activityResultID.Value == TerminateResultID))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToCloseTask };
            }

            return new ValidationResult
            {
                Success = true
            };
        }

        /// <summary>
        /// The Method to do what the User decieded
        /// </summary>
        /// <param name="data">The Data payload passed by the User</param>
        /// <param name="activityResultID">The Result ID Passed by the User to indicate which step to proceed to.</param>
        /// <returns></returns>
        public override async Task<CompletionResult> Complete(string data, Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
                activityResultID = SaveResultID;

            var task = PmnTask.GetActiveTaskForRequestActivity(_entity.ID, ID, db);

            if (activityResultID == SaveResultID)
            {
                await task.LogAsModifiedAsync(_workflow.Identity, db);
                await db.SaveChangesAsync();
                return new CompletionResult
                {
                    ResultID = SaveResultID
                };
            }
            else if (activityResultID == TerminateResultID)
            {

                db.Requests.Remove(_entity);

                if (task != null)
                {
                    db.Actions.Remove(task);
                }

                await db.SaveChangesAsync();

                return new CompletionResult
                {
                    ResultID = TerminateResultID
                };
            }
            else
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }
        }

        public override async Task Start(string comment)
        {
            var task = await PmnTask.GetActiveTaskForRequestActivityAsync(_entity.ID, ID, db);
            if (task == null)
            {
                task = db.Actions.Add(PmnTask.CreateForWorkflowActivity(_entity.ID, ID, _workflow.ID, db, CustomTaskSubject));
                await db.SaveChangesAsync();

                var analysisCenterRouting = await db.RequestDataMarts.Include(rdm => rdm.Responses).Where(rdm => rdm.RequestID == _entity.ID && rdm.RoutingType == RoutingType.AnalysisCenter).FirstOrDefaultAsync();

                Lpp.Dns.Data.Response analysisCenterResponse = null;
                if (analysisCenterRouting.Status == RoutingStatus.Draft && analysisCenterRouting.Responses.Count == 1 && analysisCenterRouting.Responses.Where(rsp => rsp.ResponseTime.HasValue == false).Any())
                {
                    //if the initial status of the routing is draft, and there is only a single response assume this is the first time hitting the analysis center.
                    //use the existing response to submit to the analysis center
                    analysisCenterResponse = analysisCenterRouting.Responses.First();
                }
                else if (analysisCenterRouting.Status != RoutingStatus.Draft)
                {
                    analysisCenterRouting.Status = RoutingStatus.Draft;
                }

                if (analysisCenterResponse == null)
                {
                    analysisCenterResponse = analysisCenterRouting.AddResponse(_workflow.Identity.ID);
                }

                if(db.Entry(task).Collection(t => t.References).IsLoaded == false)
                {
                    await db.Entry(task).Collection(t => t.References).LoadAsync();
                }

                if(task.References.Any(tr => tr.ItemID == analysisCenterResponse.ID) == false)
                {
                    //add a reference to the response to be able to link task to iteration
                    task.References.Add(new TaskReference { ItemID = analysisCenterResponse.ID, TaskID = task.ID, Type = TaskItemTypes.Response });
                }

                //use all the dp output documents to be the input documents for the AC routing
                //build a manifest for where the documents are coming from
                List<Lpp.Dns.DTO.QueryComposer.DistributedRegressionAnalysisCenterManifestItem> manifestItems = new List<DTO.QueryComposer.DistributedRegressionAnalysisCenterManifestItem>();
                var documents = await (from rd in db.RequestDocuments
                                       join rsp in db.Responses on rd.ResponseID equals rsp.ID
                                       join rdm in db.RequestDataMarts on rsp.RequestDataMartID equals rdm.ID
                                       join dm in db.DataMarts on rdm.DataMartID equals dm.ID
                                       join doc in db.Documents on rd.RevisionSetID equals doc.RevisionSetID
                                       where rsp.Count == rsp.RequestDataMart.Responses.Max(r => r.Count)
                                       && rdm.RequestID == _entity.ID
                                       && rd.DocumentType == RequestDocumentType.Output
                                       && rdm.RoutingType == RoutingType.DataPartner
                                       && doc.ID == db.Documents.Where(dd => dd.RevisionSetID == doc.RevisionSetID).OrderByDescending(dd => dd.MajorVersion).ThenByDescending(dd => dd.MinorVersion).ThenByDescending(dd => dd.BuildVersion).ThenByDescending(dd => dd.RevisionVersion).Select(dd => dd.ID).FirstOrDefault()
                                       select new
                                       {
                                           DocumentID = doc.ID,
                                           DocumentKind = doc.Kind,
                                           DocumentFileName = doc.FileName,
                                           ResponseID = rd.ResponseID,
                                           RevisionSetID = rd.RevisionSetID,
                                           RequestDataMartID = rsp.RequestDataMartID,
                                           DataMartID = rdm.DataMartID,
                                           DataPartnerIdentifier = dm.DataPartnerIdentifier,
                                           DataMart = dm.Name
                                       }).ToArrayAsync();

                // further filtering based on if a output filelist document was included needs to be done. Only the files indicated should be passed on to the analysis center
                foreach (var dpDocuments in documents.GroupBy(k => k.RequestDataMartID))
                {

                    var filelistDocument = dpDocuments.Where(d => !string.IsNullOrEmpty(d.DocumentKind) && string.Equals("DistributedRegression.FileList", d.DocumentKind, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (filelistDocument != null)
                    {

                        //only include the files indicated in the filelist document
                        using (var ds = new Lpp.Dns.Data.Documents.DocumentStream(db, filelistDocument.DocumentID))
                        using (var reader = new System.IO.StreamReader(ds))
                        {
                            //read the header line
                            reader.ReadLine();

                            string line, filename;
                            bool includeInDistribution = false;
                            while (!reader.EndOfStream)
                            {
                                line = reader.ReadLine();
                                string[] split = line.Split(',');
                                if (split.Length > 0)
                                {
                                    filename = split[0].Trim();
                                    if (split.Length > 1)
                                    {
                                        includeInDistribution = string.Equals(split[1].Trim(), "1");
                                    }
                                    else
                                    {
                                        includeInDistribution = false;
                                    }

                                    if (includeInDistribution == false)
                                        continue;

                                    if (!string.IsNullOrEmpty(filename))
                                    {
                                        Guid? revisionSetID = dpDocuments.Where(d => string.Equals(d.DocumentFileName, filename, StringComparison.OrdinalIgnoreCase)).Select(d => d.RevisionSetID).FirstOrDefault();
                                        if (revisionSetID.HasValue)
                                        {
                                            db.RequestDocuments.AddRange(dpDocuments.Where(d => d.RevisionSetID == revisionSetID.Value).Select(d => new RequestDocument { DocumentType = RequestDocumentType.Input, ResponseID = analysisCenterResponse.ID, RevisionSetID = d.RevisionSetID }).ToArray());
                                            manifestItems.AddRange(dpDocuments.Where(d => d.RevisionSetID == revisionSetID.Value).Select(d => new DTO.QueryComposer.DistributedRegressionAnalysisCenterManifestItem
                                            {
                                                DocumentID = d.DocumentID,
                                                DataMart = d.DataMart,
                                                DataMartID = d.DataMartID,
                                                DataPartnerIdentifier = d.DataPartnerIdentifier,
                                                RequestDataMartID = d.RequestDataMartID,
                                                ResponseID = d.ResponseID,
                                                RevisionSetID = d.RevisionSetID
                                            }).ToArray());
                                        }
                                    }
                                }
                            }

                            reader.Close();
                        }

                    }
                    else
                    {
                        db.RequestDocuments.AddRange(dpDocuments.Select(d => new RequestDocument { DocumentType = RequestDocumentType.Input, ResponseID = analysisCenterResponse.ID, RevisionSetID = d.RevisionSetID }).ToArray());
                        manifestItems.AddRange(dpDocuments.Select(d => new DTO.QueryComposer.DistributedRegressionAnalysisCenterManifestItem
                        {
                            DocumentID = d.DocumentID,
                            DataMart = d.DataMart,
                            DataMartID = d.DataMartID,
                            DataPartnerIdentifier = d.DataPartnerIdentifier,
                            RequestDataMartID = d.RequestDataMartID,
                            ResponseID = d.ResponseID,
                            RevisionSetID = d.RevisionSetID
                        }).ToArray());
                    }
                }

                //serialize the manifest of dataparter documents to the analysis center
                byte[] buffer;
                using (var ms = new System.IO.MemoryStream())
                using (var sw = new System.IO.StreamWriter(ms))
                using (var jw = new Newtonsoft.Json.JsonTextWriter(sw))
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.Serialize(jw, manifestItems);
                    jw.Flush();

                    buffer = ms.ToArray();
                }

                //create and add the manifest file
                Document analysisCenterManifest = db.Documents.Add(new Document
                {
                    Description = "Contains information about the input documents and the datamart they came from.",
                    Name = "Internal: Analysis Center Manifest",
                    FileName = "manifest.json",
                    ItemID = task.ID,
                    Kind = DocumentKind.SystemGeneratedNoLog,
                    UploadedByID = _workflow.Identity.ID,
                    Viewable = false,
                    MimeType = "application/json",
                    Length = buffer.Length
                });

                analysisCenterManifest.RevisionSetID = analysisCenterManifest.ID;

                db.RequestDocuments.Add(new RequestDocument { DocumentType = RequestDocumentType.Input, ResponseID = analysisCenterResponse.ID, RevisionSetID = analysisCenterManifest.RevisionSetID.Value });

                await db.SaveChangesAsync();
                await db.Entry(analysisCenterRouting).ReloadAsync();

                //post the manifest content
                analysisCenterManifest.SetData(db, buffer);

                //submit the routing
                analysisCenterRouting.Status = analysisCenterResponse.Count > 1 ? RoutingStatus.Resubmitted : RoutingStatus.Submitted;
                await db.SaveChangesAsync();

                //change the status of the request to conducting analysis
                //manually override the request status using sql direct, EF does not allow update of computed
                await db.Database.ExecuteSqlCommandAsync("UPDATE Requests SET Status = @status WHERE ID = @ID", new System.Data.SqlClient.SqlParameter("@status", (int)RequestStatuses.ConductingAnalysis), new System.Data.SqlClient.SqlParameter("@ID", _entity.ID));

                await db.Entry(_entity).ReloadAsync();
            }

            if (!string.IsNullOrWhiteSpace(comment))
            {
                var cmt = db.Comments.Add(new Comment
                {
                    CreatedByID = _workflow.Identity.ID,
                    ItemID = _entity.ID,
                    Text = comment
                });

                db.CommentReferences.Add(new CommentReference
                {
                    CommentID = cmt.ID,
                    Type = DTO.Enums.CommentItemTypes.Task,
                    ItemTitle = task.Subject,
                    ItemID = task.ID
                });

                await db.SaveChangesAsync();
            }
        }
    }
}
