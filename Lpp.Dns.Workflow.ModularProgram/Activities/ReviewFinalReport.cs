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
    public class ReviewFinalReport : ActivityBase<Request>
    {
        static readonly Guid ApproveResultID = new Guid("0811D461-626F-4CCF-B1FA-5B495858C67D");
        static readonly Guid RejectResultID = new Guid("2AFFB9A9-3BC1-4039-ADD9-FE809C81C800");
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        
        public override Guid ID
        {
            get { return ModularProgramWorkflowConfiguration.ReviewFinalReportID; }
        }

        public override string Uri
        {
            get { return "/requests/details?ID=" + this._workflow.Entity.ID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Final Report Review";
            }
        }

        public override string CustomTaskSubject
        {
            get
            {
                return "Review Final Report";
            }
        }

        public override async Task<Lpp.Workflow.Engine.ValidationResult> Validate(Guid? activityResultID)
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

        public override async Task<Lpp.Workflow.Engine.CompletionResult> Complete(string data, Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
                throw new ArgumentNullException(CommonMessages.ActivityResultIDRequired);

            if (activityResultID.Value == SaveResultID)
            {
                return new CompletionResult
                {
                    ResultID = activityResultID.Value
                };
            }

            
            CompletionResult completionResult = new CompletionResult
            {
                ResultID = activityResultID.Value
            };

            var task = await PmnTask.GetActiveTaskForRequestActivityAsync(_entity.ID, ID, db);

            var originalStatus = _entity.Status;
            if (activityResultID.Value == RejectResultID)
            {
                await SetRequestStatus(DTO.Enums.RequestStatuses.PendingFinalReport);

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.PendingFinalReport);
            }
            else
            {
                await SetRequestStatus(DTO.Enums.RequestStatuses.CompleteWithReport); 

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.CompleteWithReport);

            }
            
            await MarkTaskComplete(task);

            return completionResult;
        }
    
    }
}
