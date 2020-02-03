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
    public class PrepareDraftReport : ActivityBase<Request>
    {
        static readonly Guid CompleteWorkflowResultID = new Guid("E93CED3B-4B55-4991-AF84-07058ABE315C");

        static readonly Guid CompleteResultID = new Guid("066B79BD-ECB4-43C6-8DF9-1FDB76DD2C7A");//<== does this even get hit, not on the common submit draft report ui

        static readonly Guid RedistributeResultID = new Guid("B3C959D1-E5C6-4FCD-8812-DD2EBEA468DA");
        static readonly Guid SubmitResultID = new Guid("7385973B-1C4F-4224-A13C-F148685F0217");
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

        public override Guid ID
        {
            get { return ModularProgramWorkflowConfiguration.PrepareDraftReportID; }
        }

        public override string Uri
        {
            get { return "/requests/details?ID=" + this._workflow.Entity.ID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Draft Report";
            }
        }

        public override string CustomTaskSubject
        {
            get
            {
                return "Submit Draft Report";
            }
        }

        public override async Task<Lpp.Workflow.Engine.ValidationResult> Validate(Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultIDRequired };
            }

            if (activityResultID.Value != CompleteResultID && 
                activityResultID.Value != RedistributeResultID && 
                activityResultID.Value != SubmitResultID &&
                activityResultID.Value != SaveResultID &&
                activityResultID.Value != CompleteWorkflowResultID)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultNotSupported };
            }

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow);
            
            if (activityResultID.Value == CompleteResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToTerminateWorkflow };
            }

            if ((activityResultID.Value == RedistributeResultID || activityResultID.Value == SubmitResultID) && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask))
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

            if (activityResultID.Value == SaveResultID)
            {
                
                return new CompletionResult
                {
                    ResultID = activityResultID.Value
                };
            }

            if (activityResultID.Value == CompleteResultID)
            {
                _entity.CancelledOn = DateTime.UtcNow;
                _entity.CancelledByID = _workflow.Identity.ID;

                task.Status = DTO.Enums.TaskStatuses.Cancelled;
                task.EndOn = DateTime.UtcNow;

                await db.SaveChangesAsync();

                return new CompletionResult
                {
                    ResultID = CompleteResultID
                };
            }

            var originalStatus = _entity.Status;
            if (activityResultID.Value == CompleteWorkflowResultID) 
            {
                await SetRequestStatus(DTO.Enums.RequestStatuses.Complete);

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.Complete);

                await MarkTaskComplete(task);


            }
            else if (activityResultID.Value == RedistributeResultID)
            {
                //goes back to the distribute step

                await SetRequestStatus(DTO.Enums.RequestStatuses.Resubmitted);

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.Resubmitted);
            }
            else if (activityResultID.Value == SubmitResultID)
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
