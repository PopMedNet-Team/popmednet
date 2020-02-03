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
using Lpp.Dns.DTO.Enums;
using Lpp.Utilities.Logging;

namespace Lpp.Dns.Workflow.ModularProgram.Activities
{
    public class SpecificationReview : ActivityBase<Request>
    {
        static readonly Guid RejectResultID = new Guid("AD6A9E17-936F-431A-A5D6-97B37B7C0796");
        static readonly Guid ApproveResultID = new Guid("74294C54-05C2-4F97-BA35-FAC7C7E6F61A");
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

        public override Guid ID
        {
            get { return ModularProgramWorkflowConfiguration.SpecificationReviewID; }
        }

        public override string Uri
        {
            get { return "/requests/details?ID=" + this._workflow.Entity.ID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Specifications Review";
            }
        }

        public override string CustomTaskSubject
        {
            get
            {
                return "Review Specifications";
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
            {
                throw new ArgumentNullException(CommonMessages.ActivityResultIDRequired);
            }

            if (activityResultID.Value != ApproveResultID &&
                activityResultID.Value != RejectResultID &&
                activityResultID.Value != SaveResultID)
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }

            var originalStatus = _entity.Status;
            if (activityResultID.Value == ApproveResultID)
            {
                await SetRequestStatus(RequestStatuses.PendingPreDistributionTesting);

                await NotifyRequestStatusChanged(originalStatus, RequestStatuses.PendingPreDistributionTesting);
            }
            else if (activityResultID.Value == RejectResultID)
            {
                await SetRequestStatus(RequestStatuses.PendingWorkingSpecification);

                await NotifyRequestStatusChanged(originalStatus, RequestStatuses.PendingWorkingSpecification);
            }

            if (activityResultID.Value != SaveResultID)
            {
                var task = await PmnTask.GetActiveTaskForRequestActivityAsync(_workflow.Entity.ID, ID, db);
                await MarkTaskComplete(task);
            }

            return new CompletionResult
            {
                ResultID = activityResultID.Value
            };
        }

    }
}
