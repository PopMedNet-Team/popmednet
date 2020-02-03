using Lpp.Utilities;
using Lpp.Dns.Data;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO.Security;


namespace Lpp.Dns.Workflow.Default.Activities
{
    public class RequestReview : ActivityBase<Request>
    {
        private readonly Guid ApproveResultID = new Guid("50C60001-891F-40E6-B95F-A3B600E25C2B");
        private readonly Guid RejectResultID = new Guid("EA120001-7A35-4829-9F2D-A3B600E25013");
        private readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

        public override Guid ID
        {
            get { return DefaultWorkflowConfiguration.ReviewRequestActivityID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Request Review";
            }
        }

        public override string Uri
        {
            get { return "requests/details?ID=" + _workflow.Entity.ID; }
        }

        public async override Task<ValidationResult> Validate(Guid? activityResultID)
        {
            var errors = new StringBuilder();

            if (activityResultID == null)
                errors.AppendHtmlLine(CommonMessages.ActivityResultIDRequired);
            
            if (_entity.SubmittedOn == null)
                errors.AppendHtmlLine("Cannot approve or reject a request that has not been submitted");

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask);

            var allowApproveRejectSubmission = await ApproveRejectSubmission();
            if (!permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask) || ! allowApproveRejectSubmission)
                errors.AppendHtmlLine(CommonMessages.RequirePermissionToApproveOrRejectRequestSubmission);

            if (errors.Length > 0)
            {
                return new ValidationResult {
                    Success = false,
                    Errors = errors.ToString()
                };
            }
            else
            {
                return new ValidationResult {
                    Success = true
                };
            }
        }

        public async override Task<CompletionResult> Complete(string data, Guid? activityResultID)
        {   
            if (activityResultID.Value == SaveResultID)
            {
                return new CompletionResult
                {
                    ResultID = SaveResultID
                };
            }

            var task = PmnTask.GetActiveTaskForRequestActivity(_entity.ID, ID, db);

            await db.LoadCollection(_entity, (r) => r.DataMarts);

            if (activityResultID.Value == ApproveResultID)
            {

                var responses = await db.RequestDataMarts.Where(rdm => rdm.RequestID == _entity.ID && rdm.Status == DTO.Enums.RoutingStatus.AwaitingRequestApproval).SelectMany(rdm => rdm.Responses.Where(rsp => rsp.Count == rdm.Responses.Max(rr => rr.Count))).ToArrayAsync();
                var previousTask = await (from a in db.Actions
                                    join ar in db.ActionReferences on a.ID equals ar.TaskID
                                    where ar.ItemID == _entity.ID && a.Status == DTO.Enums.TaskStatuses.Complete select a).FirstOrDefaultAsync();
                var document = await db.Documents.Where(x => x.ItemID == previousTask.ID).FirstOrDefaultAsync();
                foreach (var dm in _entity.DataMarts.Where(dm => dm.Status == DTO.Enums.RoutingStatus.AwaitingRequestApproval))
                {
                    dm.Status = DTO.Enums.RoutingStatus.Submitted;

                    //var currentResponse = db.Responses.FirstOrDefault(r => r.RequestDataMartID == dm.ID && r.Count == r.RequestDataMart.Responses.Max(rr => rr.Count));
                    var currentResponse = responses.Where(rsp => rsp.RequestDataMartID == dm.ID).FirstOrDefault();
                    if (currentResponse == null)
                    {
                        currentResponse = db.Responses.Add(new Response { RequestDataMartID = dm.ID });
                    }
                    currentResponse.SubmittedByID = _workflow.Identity.ID;
                    currentResponse.SubmittedOn = DateTime.UtcNow;

                    db.RequestDocuments.Add(new RequestDocument { RevisionSetID = document.RevisionSetID.Value, ResponseID = currentResponse.ID, DocumentType = DTO.Enums.RequestDocumentType.Input });
                }

                _entity.Status = DTO.Enums.RequestStatuses.Submitted;

                await db.SaveChangesAsync();
                await db.Entry(_entity).ReloadAsync();

                await SetRequestStatus(DTO.Enums.RequestStatuses.Submitted);

                await MarkTaskComplete(task);

                return new CompletionResult
                {
                    ResultID = ApproveResultID
                };
            }
            else if (activityResultID.Value == RejectResultID)
            {
                //rejecting prior to submission terminates the request.

                foreach (var dm in _entity.DataMarts.Where(dm => dm.Status == DTO.Enums.RoutingStatus.AwaitingRequestApproval))
                {
                    dm.Status = DTO.Enums.RoutingStatus.RequestRejected;
                }

                await db.SaveChangesAsync();
                await db.Entry(_entity).ReloadAsync();

                _entity.RejectedByID = _workflow.Identity.ID;
                _entity.RejectedOn = DateTime.UtcNow;
                _entity.WorkFlowActivityID = DefaultWorkflowConfiguration.TerminateRequestID;                

                await SetRequestStatus(DTO.Enums.RequestStatuses.RequestRejected);            

                //create a completed task to show the request was rejected.
                PmnTask rejectedTask = db.Actions.Add(PmnTask.CreateForWorkflowActivity(_entity.ID, DefaultWorkflowConfiguration.TerminateRequestID, _workflow.ID, db));
                rejectedTask.Subject = "Request Rejected Prior to Submission";
                rejectedTask.Status = DTO.Enums.TaskStatuses.Complete;
                rejectedTask.EndOn = DateTime.UtcNow;

                if (!data.IsNullOrEmpty())
                {
                    rejectedTask.Body = data.ToStringEx();

                    var cmt = db.Comments.Add(new Comment
                    {
                        CreatedByID = _workflow.Identity.ID,
                        ItemID = _entity.ID,
                        Text = data.ToStringEx()
                    });

                    db.CommentReferences.Add(new CommentReference { 
                        CommentID = cmt.ID,
                        Type = DTO.Enums.CommentItemTypes.Task,
                        ItemTitle = rejectedTask.Subject,
                        ItemID = rejectedTask.ID
                    });
                }

                await MarkTaskComplete(task);

                return null;
            }
            else
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }
        }
    }
}
