using Lpp.Dns.Data;
using Lpp.Dns.DTO.Security;
using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Workflow.DistributedRegression.Activities
{
    /// <summary>
    /// The Second Activity for the Distributed Regression Workflow.  It allows for Altering DataPartners Responding to the Request, Altering Request MetaData, Completing The Request itself,
    /// and Terminating the Request
    /// </summary>
    public class CompleteDistribution : ActivityBase<Request>
    {
        /// <summary>
        /// The Result ID passed by the User to indicate to Redistribute the Request.  This sets the Request status back to Submitted
        /// </summary>
        private static readonly Guid RedistributeResultID = new Guid("5C5E0001-10A6-4992-A8BE-A3F4012D5FEB");
        /// <summary>
        /// The Result ID passed by the User to Indicate that the Response Time has been altered.  This does not alter the Request Status or Task in any way
        /// </summary>
        private static readonly Guid BulkEditResultID = new Guid("4F7E1762-E453-4D12-8037-BAE8A95523F7");
        /// <summary>
        /// The Result ID passed by the User to Indicate that DataPartners have been added to the Request to Respond to.  This does not alter the Request Status or Task if all DataPartners 
        /// havent responded
        /// </summary>
        private static readonly Guid AddDatamartsResultID = new Guid("15BDEF13-6E86-4E0F-8790-C07AE5B798A8");
        /// <summary>
        /// The Result ID passed by the User to Indicate that DataPartners have been Removed from the Request.  This does not alter the Request Status or Task if all DataPartners 
        /// havent responded
        /// </summary>
        private static readonly Guid RemoveDatamartsResultID = new Guid("5E010001-1353-44E9-9204-A3B600E263E9");
        /// <summary>
        /// The Result ID passed by the User to Indicate that the Routing Status has been altered.  This does not alter the Request Status or Task if all the DataPartners
        /// havent responded
        /// </summary>
        private static readonly Guid CompleteRoutingResultID = new Guid("8A68399F-D562-4A98-87C9-195D3D83A103");
        /// <summary>
        /// This Result ID is not passed by the User but is determined if it is the correct result ID based on the IF for CompleteRoutingResultID.    This does alter the Request Status and closes the Task 
        /// </summary>
        private static readonly Guid CompleteRoutingResultToAC = new Guid("7BC6CF23-D62D-4958-B0DA-78B089A23552");
        /// <summary>
        /// The Result ID passed by the User to Indicate that only the Request Metadata has been altered.  This does not alter the Request Status or Task.
        /// </summary>
        private static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        /// <summary>
        /// The Result ID passed by the User to indicate that the Request has been Terminated.  This closes the current Task and Sets the Request Status to cancelled
        /// </summary>
        private static readonly Guid TerminateResultID = new Guid("53579F36-9D20-47D9-AC33-643D9130080B");
        

        /// <summary>
        /// Gets the Name of the Current Activity
        /// </summary>
        public override string ActivityName
        {
            get
            {
                return "Complete Distribution";
            }
        }

        /// <summary>
        /// The ID of the Current Activity
        /// </summary>
        public override Guid ID
        {
            get
            {
                return new Guid("D0E659B8-1155-4F44-9728-B4B6EA4D4D55");
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
        /// The String that shows in the Task Subject Window
        /// </summary>
        public override string CustomTaskSubject
        {
            get
            {
                return "Complete Distribution";
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
                activityResultID = AddDatamartsResultID;

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask);

            if (!permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask) 
                && (activityResultID.Value == SaveResultID 
                || activityResultID.Value == RedistributeResultID
                || activityResultID.Value == BulkEditResultID
                || activityResultID.Value == AddDatamartsResultID
                || activityResultID.Value == RemoveDatamartsResultID
                || activityResultID.Value == CompleteRoutingResultID
                || activityResultID.Value == TerminateResultID))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToEditTask };
            }

            if (!permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask) 
                && (activityResultID.Value == CompleteRoutingResultID
                || activityResultID.Value == TerminateResultID))
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
            else if (activityResultID == RedistributeResultID)
            {
                if (!await db.HasPermissions<Project>(_workflow.Identity, _entity.ProjectID, Lpp.Dns.DTO.Security.PermissionIdentifiers.Project.ResubmitRequests))
                {
                    throw new System.Security.SecurityException(CommonMessages.RequirePermissionToResubmitRequest, Lpp.Dns.DTO.Security.PermissionIdentifiers.Project.ResubmitRequests.GetType());
                }

                ResubmitRoutingsModel resubmitModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResubmitRoutingsModel>(data);

                var datamarts = await (
                            from dm in db.RequestDataMarts.Include(dm => dm.Responses)
                            where dm.RequestID == _entity.ID
                            && dm.Responses.Any(r => resubmitModel.Responses.Contains(r.ID) || (r.ResponseGroupID.HasValue && resubmitModel.Responses.Contains(r.ResponseGroupID.Value)))
                            select dm
                    ).ToArrayAsync();


                //for the last completed routing from the AC
                //look for the file_list.csv document, if does not exist use all files
                //if file_list.csv exists, only use the documents it specifies

                List<Guid> documentRevisionSetIDs = new List<Guid>();

                var previousInputRequestDocuments = await(from d in db.Documents.AsNoTracking()
                                                    join reqDoc in (from rd in db.RequestDocuments
                                                                    where rd.DocumentType == DTO.Enums.RequestDocumentType.Output
                                                                    && rd.Response.RequestDataMart.RoutingType == DTO.Enums.RoutingType.AnalysisCenter
                                                                    && rd.Response.Count == rd.Response.RequestDataMart.Responses.Where(rsp => rsp.RespondedByID.HasValue).Max(rsp => rsp.Count)
                                                                    && rd.Response.RequestDataMart.RequestID == _entity.ID
                                                                    select rd) on d.RevisionSetID equals reqDoc.RevisionSetID
                                                    where d.ID == db.Documents.Where(dd => dd.RevisionSetID == d.RevisionSetID).OrderByDescending(dd => dd.MajorVersion).ThenByDescending(dd => dd.MinorVersion).ThenByDescending(dd => dd.BuildVersion).ThenByDescending(dd => dd.RevisionVersion).Select(dd => dd.ID).FirstOrDefault()
                                                    select d).ToArrayAsync();

                // look for the first document with kind == "DistributedRegression.FileList".
                Document fileListDocument = previousInputRequestDocuments.Where(d => !string.IsNullOrEmpty(d.Kind) && string.Equals("DistributedRegression.FileList", d.Kind, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (fileListDocument != null)
                {
                    //only include the files indicated in the manifest
                    byte[] fileListBytes = fileListDocument.GetData(db);
                    using (var ms = new System.IO.MemoryStream(fileListBytes))
                    using(var reader = new System.IO.StreamReader(ms))
                    {
                        //read the header line
                        reader.ReadLine();

                        string line, filename;
                        bool includeInDistribution = false;
                        while(!reader.EndOfStream)
                        {
                            line = reader.ReadLine();
                            string[] split = line.Split(',');
                            if(split.Length > 0)
                            {
                                filename = split[0].Trim();
                                if(split.Length > 1)
                                {
                                    includeInDistribution = string.Equals(split[1].Trim(), "1");
                                }else
                                {
                                    includeInDistribution = false;
                                }

                                if (includeInDistribution == false)
                                    continue;

                                if (!string.IsNullOrEmpty(filename))
                                {
                                    Guid? revisionSetID = previousInputRequestDocuments.Where(d => string.Equals(d.FileName, filename, StringComparison.OrdinalIgnoreCase)).Select(d => d.RevisionSetID).FirstOrDefault();
                                    if (revisionSetID.HasValue)
                                    {
                                        documentRevisionSetIDs.Add(revisionSetID.Value);
                                    }
                                }
                            }
                        }

                        reader.Close();
                    }
                }
                else
                {
                    //include all documents
                    documentRevisionSetIDs.AddRange(previousInputRequestDocuments.Select(d => d.RevisionSetID.Value).Distinct());
                }


                DateTime reSubmittedOn = DateTime.UtcNow;
                foreach (var dm in datamarts)
                {

                    var response = dm.AddResponse(_workflow.Identity.ID);
                    response.SubmittedOn = reSubmittedOn;
                    response.SubmitMessage = resubmitModel.ResubmissionMessage;

                    dm.Status = DTO.Enums.RoutingStatus.Resubmitted;

                    foreach (var revisionSetID in documentRevisionSetIDs)
                    {
                        db.RequestDocuments.Add(new RequestDocument { ResponseID = response.ID, RevisionSetID = revisionSetID, DocumentType = DTO.Enums.RequestDocumentType.Input });
                    }
                }

                await SetRequestStatus(DTO.Enums.RequestStatuses.Submitted);

                await task.LogAsModifiedAsync(_workflow.Identity, db);
                await db.SaveChangesAsync();
                return new CompletionResult
                {
                    ResultID = RedistributeResultID
                };
            }
            else if (activityResultID == BulkEditResultID)
            {
                await task.LogAsModifiedAsync(_workflow.Identity, db);
                await db.SaveChangesAsync();
                return new CompletionResult
                {
                    ResultID = BulkEditResultID
                };
            }
            else if (activityResultID == AddDatamartsResultID)
            {
                var modularTerm = GetAllTerms(HorizontalDistributedRegressionConfiguration.ModularProgramTermID, ParseRequestJSON()).FirstOrDefault();

                var termValues = Newtonsoft.Json.JsonConvert.DeserializeObject<ModularProgramTermValues>(modularTerm.Values["Values"].ToString());

                string[] datamartIDs = data.Split(',');

                var allTasks = await db.ActionReferences.Where(tr => tr.ItemID == _entity.ID
                                                  && tr.Type == DTO.Enums.TaskItemTypes.Request
                                                  && tr.Task.Type == DTO.Enums.TaskTypes.Task
                                                 )
                                                 .Select(tr => tr.Task.ID).ToArrayAsync();

                var attachments = await (from doc in db.Documents.AsNoTracking()
                                         join x in (
                                                 db.Documents.Where(dd => allTasks.Contains(dd.ItemID))
                                                 .GroupBy(k => k.RevisionSetID)
                                                 .Select(k => k.OrderByDescending(d => d.MajorVersion).ThenByDescending(d => d.MinorVersion).ThenByDescending(d => d.BuildVersion).ThenByDescending(d => d.RevisionVersion).Select(y => y.ID).Distinct().FirstOrDefault())
                                             ) on doc.ID equals x
                                         where allTasks.Contains(doc.ItemID) && doc.Kind == "Attachment.Input"
                                         orderby doc.ItemID descending, doc.RevisionSetID descending, doc.CreatedOn descending
                                         select doc).ToArrayAsync();

                foreach (var guid in datamartIDs)
                {
                    Guid dmGuid = new Guid(guid);

                    var dm = RequestDataMart.Create(_entity.ID, dmGuid, _workflow.Identity.ID);
                    dm.Status = DTO.Enums.RoutingStatus.Submitted;
                    dm.DueDate = _entity.DueDate;
                    dm.Priority = _entity.Priority;
                    dm.RoutingType = DTO.Enums.RoutingType.DataPartner;
                    _entity.DataMarts.Add(dm);

                    Response rsp = dm.Responses.OrderByDescending(r => r.Count).FirstOrDefault();
                    //add the request document associations
                    foreach (var revisionSetID in termValues.Documents.Select(d => d.RevisionSetID))
                    {
                        db.RequestDocuments.Add(new RequestDocument { DocumentType = DTO.Enums.RequestDocumentType.Input, ResponseID = rsp.ID, RevisionSetID = revisionSetID });
                    }

                    foreach (var attachment in attachments)
                    {
                        db.RequestDocuments.Add(new RequestDocument { RevisionSetID = attachment.RevisionSetID.Value, ResponseID = rsp.ID, DocumentType = DTO.Enums.RequestDocumentType.AttachmentInput });
                    }

                }

                await task.LogAsModifiedAsync(_workflow.Identity, db);
                await db.SaveChangesAsync();
                return new CompletionResult
                {
                    ResultID = AddDatamartsResultID
                };
            }
            else if (activityResultID == RemoveDatamartsResultID)
            {
                Guid[] guids = data.Split(',').Select(s => Guid.Parse(s)).ToArray();
                var routings = await db.RequestDataMarts.Where(dm => guids.Contains(dm.ID)).ToArrayAsync();

                foreach (var routing in routings)
                {
                    routing.Status = DTO.Enums.RoutingStatus.Canceled;
                }

                await task.LogAsModifiedAsync(_workflow.Identity, db);
                await db.SaveChangesAsync();

                var originalStatus = _entity.Status;
                await db.SaveChangesAsync();

                await db.Entry(_entity).ReloadAsync();

                if (originalStatus != DTO.Enums.RequestStatuses.Complete && _entity.Status == DTO.Enums.RequestStatuses.Complete)
                {
                    await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.Complete);
                }

                return new CompletionResult
                {
                    ResultID = RemoveDatamartsResultID
                };
            }
            else if (activityResultID == CompleteRoutingResultID)
            {

                var uiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<WebResponseModel>(data);

                var routing = await db.RequestDataMarts.Include("Request").SingleOrDefaultAsync(dm => dm.ID == uiResponse.RequestDM && dm.RequestID == _entity.ID);
                var response = await db.Responses.SingleOrDefaultAsync(res => res.ID == uiResponse.Response && res.RequestDataMartID == uiResponse.RequestDM);
                response.ResponseTime = DateTime.UtcNow;
                response.RespondedByID = _workflow.Identity.ID;
                response.ResponseMessage = uiResponse.Comment;
                var originalRequestStatus = routing.Request.Status;
                var originalStatus = routing.Status;


                //We should only update the routing status if it is not already complete or modified.
                if (originalStatus != DTO.Enums.RoutingStatus.Completed && originalStatus != DTO.Enums.RoutingStatus.ResultsModified)
                    routing.Status = DTO.Enums.RoutingStatus.Completed;

                if (routing.Status == DTO.Enums.RoutingStatus.Completed || routing.Status == DTO.Enums.RoutingStatus.ResultsModified)
                {
                    try
                    {
                        var trackingTableProcessor = new DistributedRegressionTrackingTableProcessor(db);
                        await trackingTableProcessor.Process(response);
                    }
                    catch (Exception ex)
                    {
                        //should not block if fails

                    }
                }

                await db.SaveChangesAsync();
                await db.Entry<RequestDataMart>(routing).ReloadAsync();
                await db.Entry<Request>(_entity).ReloadAsync();
                var completeStatuses = new[] {
                    Lpp.Dns.DTO.Enums.RoutingStatus.Completed,
                    Lpp.Dns.DTO.Enums.RoutingStatus.ResultsModified,
                    Lpp.Dns.DTO.Enums.RoutingStatus.RequestRejected,
                    Lpp.Dns.DTO.Enums.RoutingStatus.ResponseRejectedBeforeUpload,
                    Lpp.Dns.DTO.Enums.RoutingStatus.ResponseRejectedAfterUpload,
                    Lpp.Dns.DTO.Enums.RoutingStatus.AwaitingResponseApproval
                };

                

                var incompletedRoutings = await db.RequestDataMarts.Where(dm => dm.RequestID == _entity.ID
                                                                && dm.RoutingType != DTO.Enums.RoutingType.AnalysisCenter
                                                                && !completeStatuses.Contains(dm.Status)).CountAsync();

                if (incompletedRoutings == 0)
                {
                    try
                    {
                        var routingProcessor = new DistributedRegressionRoutingProcessor(db, _workflow.Identity.ID);
                        await routingProcessor.Process(routing);

                        return new CompletionResult
                        {
                            ResultID = CompleteRoutingResultToAC
                        };
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                else
                {
                    await task.LogAsModifiedAsync(_workflow.Identity, db);
                    await db.SaveChangesAsync();

                    return new CompletionResult
                    {
                        ResultID = CompleteRoutingResultID
                    };
                }
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
    }

    public class ResubmitRoutingsModel
    {
        public IEnumerable<Guid> Responses { get; set; }

        public string ResubmissionMessage { get; set; }
    }

    public class WebResponseModel
    {
        public Guid Response { get; set; }
        public Guid RequestDM { get; set; }
        public string Comment { get; set; }
    }
}
