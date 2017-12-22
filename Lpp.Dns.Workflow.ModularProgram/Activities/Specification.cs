using Lpp.Dns.Data;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.Logging;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Workflow.ModularProgram.Activities
{
    public class Specification : ActivityBase<Request>
    {
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        static readonly Guid SubmitResultID = new Guid("C8260E90-2C8B-435A-85C8-372B021C3E9F");
        static readonly Guid TerminateResultID = new Guid("53579F36-9D20-47D9-AC33-643D9130080B");
        static readonly Guid ModifyResultID = new Guid("E57E6B65-140F-452B-95FF-04BDB16BCD2D");

        private const string DocumentKind = "Lpp.Dns.Workflow.ModularProgram.Activities.Specification";

        public override Guid ID
        {
            get { return ModularProgramWorkflowConfiguration.SpecificationID; }
        }

        public override string Uri
        {
            get { return "/requests/details?ID=" + this._workflow.Entity.ID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Specifications";
            }
        }

        public override string CustomTaskSubject
        {
            get
            {
                return "Submit Specifications";
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
                activityResultID.Value != SubmitResultID && 
                activityResultID.Value != ModifyResultID )
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

            if ((activityResultID.Value == SubmitResultID || activityResultID.Value == ModifyResultID) && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask))
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

            if (activityResultID.Value == SaveResultID || 
                activityResultID.Value == SubmitResultID || 
                activityResultID.Value == ModifyResultID)
            {
                var originalStatus = _entity.Status;
                if (activityResultID.Value == SubmitResultID)
                {
                    await SetRequestStatus(DTO.Enums.RequestStatuses.SpecificationsPendingReview);

                    await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.SpecificationsPendingReview);

                    await MarkTaskComplete(task);
                }
                else if (activityResultID.Value == ModifyResultID)
                {
                    await SetRequestStatus(DTO.Enums.RequestStatuses.PendingWorkingSpecification);

                    await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.PendingWorkingSpecification);

                    await MarkTaskComplete(task);
                }
                else
                {
                    await task.LogAsModifiedAsync(_workflow.Identity, db);
                    await db.SaveChangesAsync();
                }                

                return new CompletionResult
                {
                    ResultID = activityResultID.Value
                };
            }
            else if (activityResultID.Value == TerminateResultID)
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
            else
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }

        }


    }
}
