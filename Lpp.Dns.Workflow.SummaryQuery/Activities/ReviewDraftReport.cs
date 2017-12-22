using Lpp.Dns.Data;
using Lpp.Utilities.Logging;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Workflow.SummaryQuery.Activities
{
    public class ReviewDraftReport : ActivityBase<Request>
    {
        static readonly Guid RejectResultID = new Guid("687360E2-8389-48E3-A3FE-71248F0D6192");
        static readonly Guid ApproveResultID = new Guid("36F8F9BA-849A-493F-A9FA-B443370EF5AD");
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

        public override Guid ID
        {
            get { return SummaryQueryWorkflowConfiguration.ReviewDraftReportID; }
        }

        public override string Uri
        {
            get { return "/requests/details?ID=" + _entity.ID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Draft Report Review";
            }
        }

        public override string CustomTaskSubject
        {
            get
            {
                return "Review Draft Report";
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
                return new ValidationResult{ Success = false, Errors = CommonMessages.ActivityResultNotSupported };
            }

            if (!(await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, Lpp.Dns.DTO.Security.PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask)).Any())
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToCloseTask };
            }

            return new ValidationResult
            {
                Success = true
            };
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

            if (activityResultID.Value == SaveResultID)
            {
                return new CompletionResult
                {
                    ResultID = SaveResultID
                };
            }

            var originalStatus = _entity.Status;
            await SetRequestStatus(activityResultID.Value == ApproveResultID ? DTO.Enums.RequestStatuses.PendingFinalReport : DTO.Enums.RequestStatuses.PendingDraftReport);

            await NotifyRequestStatusChanged(originalStatus,
                                             activityResultID.Value == ApproveResultID ? DTO.Enums.RequestStatuses.PendingFinalReport : DTO.Enums.RequestStatuses.PendingDraftReport
                                            );

            var task = PmnTask.GetActiveTaskForRequestActivity(_entity.ID, ID, db);
            await MarkTaskComplete(task);

            return new CompletionResult
            {
                ResultID = activityResultID.Value
            };
        }
    }
}
