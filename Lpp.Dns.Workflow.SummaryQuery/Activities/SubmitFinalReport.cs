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
    public class SubmitFinalReport : ActivityBase<Request>
    {
        //this is suppose to be complete without report
        static readonly Guid CompleteWithoutReportResultID = new Guid("E93CED3B-4B55-4991-AF84-07058ABE315C");
        //submit final report and move to report review
        static readonly Guid SubmitReportForReviewResultID = new Guid("0CF45F91-6F2C-4283-BDC2-0896B552694A");
        //go back to review draft report
        static readonly Guid ReopenSubmitDraftReportResultID = new Guid("ECCBF404-B3BA-4C5E-BB6E-388725938DC3");
        //save no change to current activity
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

        //don't know what this one is for, complete with report?
        //static readonly Guid CompleteResultID = new Guid("B38C1515-BF25-4179-BA09-9F811E0053D8");
        

        public override Guid ID
        {
            get { return SummaryQueryWorkflowConfiguration.SubmitFinalReportID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Final Report";
            }
        }

        public override string Uri
        {
            get { return "/requests/details?ID=" + _entity.ID; }
        }

        public override string CustomTaskSubject
        {
            get
            {
                return "Submit Final Report";
            }
        }

        public override async Task<ValidationResult> Validate(Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultIDRequired };
            }

            if (//activityResultID.Value != CompleteResultID && 
                activityResultID.Value != ReopenSubmitDraftReportResultID && 
                activityResultID.Value != SubmitReportForReviewResultID &&
                activityResultID.Value != SaveResultID && activityResultID.Value != CompleteWithoutReportResultID)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultNotSupported };
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

            if (//activityResultID.Value != CompleteResultID &&
                activityResultID.Value != ReopenSubmitDraftReportResultID &&
                activityResultID.Value != SubmitReportForReviewResultID &&
                activityResultID.Value != SaveResultID && 
                activityResultID.Value != CompleteWithoutReportResultID)
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

            Guid destinationActivityID = Guid.Empty;
            DTO.Enums.RequestStatuses destinationRequestStatus = DTO.Enums.RequestStatuses.Complete;
            var originalStatus = _entity.Status;

            if (activityResultID.Value == CompleteWithoutReportResultID)
            {
                await SetRequestStatus(DTO.Enums.RequestStatuses.Complete);
                destinationActivityID = SummaryQueryWorkflowConfiguration.CompletedID;

            }
            else if (activityResultID.Value == SubmitReportForReviewResultID)
            {
                await SetRequestStatus(DTO.Enums.RequestStatuses.FinalReportPendingReview);
                destinationActivityID = SummaryQueryWorkflowConfiguration.ReviewFinalReportID;
                destinationRequestStatus = DTO.Enums.RequestStatuses.FinalReportPendingReview;
            }
            else if (activityResultID.Value == ReopenSubmitDraftReportResultID)
            {
                await SetRequestStatus(DTO.Enums.RequestStatuses.PendingDraftReport);
                destinationActivityID = SummaryQueryWorkflowConfiguration.SubmitDraftReportID;
                destinationRequestStatus = DTO.Enums.RequestStatuses.PendingDraftReport;
            }

            var task = PmnTask.GetActiveTaskForRequestActivity(_entity.ID, ID, db);
            await MarkTaskComplete(task);

            await NotifyRequestStatusChanged(originalStatus, destinationRequestStatus);

            return new CompletionResult
            {
                ResultID = activityResultID.Value
            };

        }
    }
}
