using Lpp.Dns.Data;
using Lpp.Utilities;
using Lpp.Dns.DTO.Security;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Logging;

namespace Lpp.Dns.Workflow.SummaryQuery.Activities
{
    public class RequestReview : ActivityBase<Request>
    {
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        static readonly Guid TerminateResultID = new Guid("53579F36-9D20-47D9-AC33-643D9130080B");
        static readonly Guid RejectResultID = new Guid("EA120001-7A35-4829-9F2D-A3B600E25013");
        static readonly Guid ApproveResultID = new Guid("4AE61A78-CE31-4A01-807F-DB18A535E4E0");

        public override Guid ID
        {
            get { return SummaryQueryWorkflowConfiguration.ReviewRequestID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Request Form Review";
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
                return "Review Request Form";
            }
        }

        public override async Task<ValidationResult> Validate(Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultIDRequired };
            }

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow);

            if (activityResultID.Value == SaveResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToEditTask };
            }
            var allowApprove = await ApproveRejectSubmission();
            if (activityResultID.Value == ApproveResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask) && !allowApprove)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToCloseTask };
            }
            if (activityResultID.Value == TerminateResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToTerminateWorkflow };
            }

            return new ValidationResult
            {
                Success = true
            };
        }

        public override async Task<CompletionResult> Complete(string data, Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
                throw new ArgumentNullException(CommonMessages.ActivityResultIDRequired);


            if (activityResultID.Value == SaveResultID)
            {
                return new CompletionResult {
                    ResultID = SaveResultID
                };
            } 
            else if (activityResultID.Value == RejectResultID)
            {
                _entity.CancelledByID = _workflow.Identity.ID;
                _entity.CancelledOn = DateTime.UtcNow;
                _entity.Status = DTO.Enums.RequestStatuses.RequestRejected;

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
            } else if (activityResultID.Value == ApproveResultID) {

                var filters = new ExtendedQuery
                {
                    Projects = (a) => a.ProjectID == _entity.ProjectID,
                    ProjectOrganizations = a => a.ProjectID == _entity.ProjectID && a.OrganizationID == _entity.OrganizationID,
                    Organizations = a => a.OrganizationID == _entity.OrganizationID,
                    Users = a => a.UserID == _entity.CreatedByID
                };

                var permissions = await db.HasGrantedPermissions<Request>(_workflow.Identity, _entity, filters, PermissionIdentifiers.Request.ApproveRejectSubmission);

                if (!permissions.Contains(PermissionIdentifiers.Request.ApproveRejectSubmission))
                {
                    throw new SecurityException(CommonMessages.RequirePermissionToApproveOrRejectRequestSubmission);
                }

                var originalStatus = _entity.Status;
                await SetRequestStatus(DTO.Enums.RequestStatuses.RequestPendingDistribution);

                await db.LoadCollection(_entity, (r) => r.DataMarts);

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.RequestPendingDistribution);

                var task = PmnTask.GetActiveTaskForRequestActivity(_entity.ID, ID, db);
                await MarkTaskComplete(task);

                return new CompletionResult
                {
                    ResultID = ApproveResultID
                };
            }
            else
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }

        }
    }
}
