using Lpp.Utilities;
using Lpp.Dns.Data;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO.Security;
using System.Data.Entity;

namespace Lpp.Dns.Workflow.DataChecker.Activities
{
    public class RequestReview : ActivityBase<Request>
    {
        private readonly Guid ApproveResultID = new Guid("50C60001-891F-40E6-B95F-A3B600E25C2B");
        private readonly Guid RejectResultID = new Guid("EA120001-7A35-4829-9F2D-A3B600E25013");
        private readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

        public override Guid ID
        {
            get { return DataCheckerWorkflowConfiguration.ReviewRequestActivityID; }
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
            if (!permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask) || !allowApproveRejectSubmission)
                errors.AppendHtmlLine(CommonMessages.RequirePermissionToApproveOrRejectRequestSubmission);

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

        

        public async override Task<CompletionResult> Complete(string data, Guid? activityResultID)
        {
            await db.LoadCollection(_entity, (r) => r.DataMarts);

            if (activityResultID.Value == SaveResultID)
            {
                return new CompletionResult
                {
                    ResultID = SaveResultID
                };
            }

            if (activityResultID.Value == ApproveResultID)
            {
                var responses = await db.RequestDataMarts.Where(rdm => rdm.RequestID == _entity.ID && rdm.Status == DTO.Enums.RoutingStatus.AwaitingRequestApproval).SelectMany(rdm => rdm.Responses.Where(rsp => rsp.Count == rdm.Responses.Max(rr => rr.Count))).ToArrayAsync();
                var previousTask = await (from a in db.Actions
                                          join ar in db.ActionReferences on a.ID equals ar.TaskID
                                          where ar.ItemID == _entity.ID && a.Status == DTO.Enums.TaskStatuses.Complete
                                          select a).FirstOrDefaultAsync();
                var document = await db.Documents.Where(x => x.ItemID == previousTask.ID).OrderByDescending(p => p.CreatedOn).FirstOrDefaultAsync();
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

                var task = PmnTask.GetActiveTaskForRequestActivity(_entity.ID, ID, db);
                if (task != null)
                {
                    task.Status = DTO.Enums.TaskStatuses.Complete;
                    task.EndOn = DateTime.UtcNow;
                }

                await db.SaveChangesAsync();

                return new CompletionResult
                {
                    ResultID = ApproveResultID
                };
            }
            else if (activityResultID.Value == RejectResultID)
            {
                foreach (var dm in _entity.DataMarts.Where(dm => dm.Status == DTO.Enums.RoutingStatus.AwaitingRequestApproval))
                    dm.Status = DTO.Enums.RoutingStatus.RequestRejected;

                _entity.Status = DTO.Enums.RequestStatuses.RequestRejected;
                _entity.RejectedByID = _workflow.Identity.ID;
                _entity.RejectedOn = DateTime.UtcNow;

                var task = PmnTask.GetActiveTaskForRequestActivity(_entity.ID, ID, db);

                var originalStatus = _entity.Status;
                await SetRequestStatus(DTO.Enums.RequestStatuses.RequestRejected);

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.RequestRejected);

                await MarkTaskComplete(task);

                await db.SaveChangesAsync();

                return new CompletionResult
                {
                    ResultID = RejectResultID
                };
            }
            else
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }
        }

    }
}
