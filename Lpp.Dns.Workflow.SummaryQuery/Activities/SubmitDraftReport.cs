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
    public class SubmitDraftReport : ActivityBase<Request>
    {
        static readonly Guid CompleteWorkflowResultID = new Guid("E93CED3B-4B55-4991-AF84-07058ABE315C");

        static readonly Guid CompleteResultID = new Guid("066B79BD-ECB4-43C6-8DF9-1FDB76DD2C7A");
        static readonly Guid RedistributeResultID = new Guid("B3C959D1-E5C6-4FCD-8812-DD2EBEA468DA");
        static readonly Guid SubmitReportResultID = new Guid("7385973B-1C4F-4224-A13C-F148685F0217");
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

        public override Guid ID
        {
            get { return SummaryQueryWorkflowConfiguration.SubmitDraftReportID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Pending Draft Report";
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
                return "Submit Draft Report";
            }
        }

        public override async Task<ValidationResult> Validate(Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultIDRequired };
            }

            if (activityResultID.Value != CompleteResultID &&
                activityResultID.Value != RedistributeResultID &&
                activityResultID.Value != SubmitReportResultID &&
                activityResultID.Value != SaveResultID && 
                activityResultID.Value != CompleteWorkflowResultID)
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
            //Handle what happens in each of the completions.
            if (!activityResultID.HasValue)
            {
                throw new ArgumentNullException(CommonMessages.ActivityResultIDRequired);
            }

            if (activityResultID.Value != CompleteResultID &&
                activityResultID.Value != RedistributeResultID &&
                activityResultID.Value != SubmitReportResultID &&
                activityResultID.Value != SaveResultID && activityResultID != CompleteWorkflowResultID)
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }

            if (activityResultID.Value == SaveResultID)
            {
                //save, no change to current activity for the request
                return new CompletionResult
                {
                    ResultID = SaveResultID
                };
            }

            var task = PmnTask.GetActiveTaskForRequestActivity(_entity.ID, ID, db);
            var originalStatus = _entity.Status;
            if (activityResultID.Value == CompleteWorkflowResultID)
            {
                await SetRequestStatus(DTO.Enums.RequestStatuses.Complete);

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.Complete);

            }
            else if (activityResultID.Value == RedistributeResultID)
            {
                //re-submits the requests and goes back to the distribute step
                //TODO: should this be resubmitting all the routes, or add as pending, or do nothing and just redirect back to view status and results?
                
                await SetRequestStatus(DTO.Enums.RequestStatuses.Resubmitted);

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.Resubmitted);
            }
            else if (activityResultID.Value == SubmitReportResultID)
            {
                //move to draft report review

                await SetRequestStatus(DTO.Enums.RequestStatuses.DraftReportPendingApproval);

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.DraftReportPendingApproval);
            }

            await MarkTaskComplete(task);          

            return new CompletionResult
            {
                ResultID = activityResultID.Value
            };

        }
    }
}
