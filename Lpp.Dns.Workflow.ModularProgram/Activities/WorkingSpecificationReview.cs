using Lpp.Dns.Data;
using Lpp.Dns.DTO.Security;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Lpp.Utilities.Logging;

namespace Lpp.Dns.Workflow.ModularProgram.Activities
{
    public class WorkingSpecificationReview : ActivityBase<Request>
    {
        static readonly Guid ApproveResultID = new Guid("982C4DCC-AB1C-4F87-83BB-E09FA8270C17");
        static readonly Guid RejectResultID = new Guid("A95899AC-F4F6-41AB-AD4B-D41E05563486");
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

        public override Guid ID
        {
            get { return ModularProgramWorkflowConfiguration.WorkingSpecificationReviewID; }
        }

        public override string Uri
        {
            get { return "/requests/details?ID=" + this._workflow.Entity.ID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Working Specifications Review";
            }
        }

        public override string CustomTaskSubject
        {
            get
            {
                return "Review Working Specifications";
            }
        }

        public override async Task<ValidationResult> Validate(Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultIDRequired };
            }

            if (activityResultID.Value != ApproveResultID &&
                activityResultID.Value != RejectResultID &&
                activityResultID.Value != SaveResultID)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultNotSupported };
            }

            if (!(await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask)).Any())
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToCloseTask };
            }

            return new ValidationResult { Success = true };
        }

        public override async Task<CompletionResult> Complete(string data, Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
                throw new ArgumentNullException(CommonMessages.ActivityResultIDRequired);

            if (activityResultID.Value == SaveResultID)
            {
                return new CompletionResult
                {
                    ResultID = SaveResultID
                };
            }

            var originalStatus = _entity.Status;
            if (activityResultID.Value == ApproveResultID)
            {
                await SetRequestStatus(DTO.Enums.RequestStatuses.PendingSpecifications);

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.PendingSpecifications);
            }
            else if (activityResultID.Value == RejectResultID)
            {
                await SetRequestStatus(DTO.Enums.RequestStatuses.PendingWorkingSpecification);

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.PendingWorkingSpecification);
            }
            else
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }            

            var task = await PmnTask.GetActiveTaskForRequestActivityAsync(_entity.ID, ID, db);
            await MarkTaskComplete(task);

            return new CompletionResult
            {
                ResultID = activityResultID.Value
            };

        }

    }
}
