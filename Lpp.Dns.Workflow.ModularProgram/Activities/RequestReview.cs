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
using System.Data.Entity;
using Lpp.Dns.DTO.Enums;
using Lpp.Utilities.Logging;

namespace Lpp.Dns.Workflow.ModularProgram.Activities
{
    public class RequestReview : ActivityBase<Request>
    {
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        static readonly Guid TerminateResultID = new Guid("546164B4-0D6D-4C26-868B-07280F627818");
        static readonly Guid ApproveResultID = new Guid("DEB04531-1635-4B32-AB0F-14C1CCF9BAFB");
        
        public override Guid ID
        {
            get { return ModularProgramWorkflowConfiguration.ReviewRequestID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Draft Pending Review";
            }
        }

        public override string Uri
        {
            get { return "/requests/details?ID=" + this._workflow.Entity.ID; }
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

            if (activityResultID.Value != TerminateResultID && 
                activityResultID.Value != SaveResultID && 
                activityResultID.Value != ApproveResultID)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultNotSupported };
            }

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow);
            
            if (activityResultID.Value == TerminateResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToTerminateWorkflow };
            }

            if (activityResultID.Value == SaveResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToEditTask };
            }

            if (activityResultID.Value == ApproveResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToCloseTask };
            }

            return new ValidationResult { Success = true };
        }

        public override async Task<CompletionResult> Complete(string data, Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
                throw new ArgumentNullException(CommonMessages.ActivityResultIDRequired);

            var task = await PmnTask.GetActiveTaskForRequestActivityAsync(_entity.ID, ID, db);

            if (activityResultID.Value == TerminateResultID)
            {
                //cancel out the request and the task, then get out.
                _entity.CancelledByID = _workflow.Identity.ID;
                _entity.CancelledOn = DateTime.UtcNow;
                task.Status = DTO.Enums.TaskStatuses.Cancelled;                

                await db.SaveChangesAsync();

                return null;
            }

            await db.SaveChangesAsync();
            

            if (activityResultID.Value == SaveResultID)
            {
                await task.LogAsModifiedAsync(_workflow.Identity, db);
                await db.SaveChangesAsync();

                return new CompletionResult {
                    ResultID = SaveResultID
                };
            } else if (activityResultID.Value == ApproveResultID) {
                var originalStatus = _entity.Status;
                await SetRequestStatus(RequestStatuses.PendingWorkingSpecification);

                await NotifyRequestStatusChanged(originalStatus, RequestStatuses.PendingWorkingSpecification);

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
