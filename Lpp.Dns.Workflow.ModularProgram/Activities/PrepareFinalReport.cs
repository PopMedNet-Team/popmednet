using Lpp.Dns.Data;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.Logging;

namespace Lpp.Dns.Workflow.ModularProgram.Activities
{
    public class PrepareFinalReport : ActivityBase<Request>
    {
        static readonly Guid CompleteWithoutReportResultID = new Guid("E93CED3B-4B55-4991-AF84-07058ABE315C");

        static readonly Guid TerminateRequestResultID =new Guid("B38C1515-BF25-4179-BA09-9F811E0053D8");
        static readonly Guid ReviewFinalReportID = new Guid("0CF45F91-6F2C-4283-BDC2-0896B552694A");
        static readonly Guid PrepareDraftReportID = new Guid("ECCBF404-B3BA-4C5E-BB6E-388725938DC3");
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

        public override Guid ID
        {
            get { return ModularProgramWorkflowConfiguration.PrepareFinalReportID; }
        }

        public override string Uri
        {
            get { return "/requests/details?ID=" + this._workflow.Entity.ID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Final Report";
            }
        }

        public override string CustomTaskSubject
        {
            get
            {
                return "Submit Final Report";
            }
        }

        public override async Task<Lpp.Workflow.Engine.ValidationResult> Validate(Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultIDRequired };
            }

            if (activityResultID.Value != TerminateRequestResultID && 
                activityResultID.Value != ReviewFinalReportID && 
                activityResultID.Value != PrepareDraftReportID &&
                activityResultID.Value != SaveResultID &&
                activityResultID.Value != CompleteWithoutReportResultID)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultNotSupported };
            }

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow);
            
            if (activityResultID.Value == TerminateRequestResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToTerminateWorkflow };
            }

            if ((activityResultID.Value == ReviewFinalReportID || activityResultID.Value == PrepareDraftReportID) && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToCloseTask };
            }

            return new ValidationResult { Success = true };
        }

        public override async Task<Lpp.Workflow.Engine.CompletionResult> Complete(string data, Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
                throw new ArgumentNullException(CommonMessages.ActivityResultIDRequired);

            var task = await PmnTask.GetActiveTaskForRequestActivityAsync(_workflow.Entity.ID, ID, db);
            if (task == null)
            {
                task = db.Actions.Add(PmnTask.CreateForWorkflowActivity(_entity.ID, ID, _workflow.ID, db));
            }

            if (activityResultID.Value == TerminateRequestResultID)
            {
                _entity.CancelledOn = DateTime.UtcNow;
                _entity.CancelledByID = _workflow.Identity.ID;

                task.Status = DTO.Enums.TaskStatuses.Cancelled;
                task.EndOn = DateTime.UtcNow;

                await db.SaveChangesAsync();

                return new CompletionResult
                {
                    ResultID = TerminateRequestResultID
                };
            }

            if (activityResultID.Value == SaveResultID)
            {
                return new CompletionResult
                {
                    ResultID = activityResultID.Value
                };
            }

            var completionResult = new CompletionResult
            {
                ResultID = activityResultID.Value
            };

            var originalStatus = _entity.Status;
            if (activityResultID.Value == CompleteWithoutReportResultID)
            {
                await SetRequestStatus(DTO.Enums.RequestStatuses.Complete);

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.Complete);

            }
            else if(activityResultID.Value == ReviewFinalReportID)
            {
                await SetRequestStatus(DTO.Enums.RequestStatuses.FinalReportPendingReview);

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.FinalReportPendingReview);
            }
            else if(activityResultID.Value == PrepareDraftReportID)
            {
                await SetRequestStatus(DTO.Enums.RequestStatuses.PendingDraftReport);

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.PendingDraftReport);
            }
            else
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }

            await MarkTaskComplete(task);


            return new CompletionResult
            {
                ResultID = activityResultID.Value
            };
        }
    
    }
}
