using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Lpp.Dns.DTO.Security;
using System.Data.SqlClient;
using Lpp.Utilities.Logging;

namespace Lpp.Dns.Workflow.ModularProgram.Activities
{
    public class DraftRequest : ActivityBase<Request>
    {
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        static readonly Guid SubmitResultID = new Guid("6248E8B1-7C7C-4959-993F-352C722821A6");
        static readonly Guid DeleteResultID = new Guid("7E8661F2-E540-4E91-A3CF-982DB52EF965");

        private const string DocumentKind = "Lpp.Dns.Workflow.ModularProgram.Activities.DraftRequest";

        
        public override Guid ID
        {
            get { return ModularProgramWorkflowConfiguration.DraftRequestID; }
        }

        public override string Uri
        {
            get { return "/requests/details?ID=" + this._workflow.Entity.ID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Request Form";
            }
        }

        public override async Task<ValidationResult> Validate(Guid? activityResultID)
        {
            //default to the save result ID if not specified.
            if (!activityResultID.HasValue)
                activityResultID = SaveResultID;

            if (activityResultID.Value != SaveResultID && 
                activityResultID.Value != DeleteResultID && 
                activityResultID.Value != SubmitResultID)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultNotSupported };
            }

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask);

            if (activityResultID.Value == SaveResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToEditTask };
            }

            if (activityResultID.Value == SubmitResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask))
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
                activityResultID = SaveResultID;

            var task = await PmnTask.GetActiveTaskForRequestActivityAsync(_entity.ID, ID, db);
            if (task == null)
            {
                task = db.Actions.Add(PmnTask.CreateForWorkflowActivity(_entity.ID, ID, _workflow.ID, db));
            }

            if (activityResultID.Value == SaveResultID || activityResultID.Value == SubmitResultID)
            {
                await db.Entry(_entity).ReloadAsync();
                _entity.Private = false;

                if (activityResultID.Value == SubmitResultID)
                {
                    _entity.SubmittedByID = _workflow.Identity.ID;

                    //Reset reject for resubmit.
                    _entity.RejectedByID = null;
                    _entity.RejectedOn = null;

                    var originalStatus = _entity.Status;
                    await SetRequestStatus(DTO.Enums.RequestStatuses.DraftReview);

                    await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.DraftReview);

                    await MarkTaskComplete(task);
                }
                else
                {
                    await task.LogAsModifiedAsync(_workflow.Identity, db);                    
                }

                return new CompletionResult
                {
                    ResultID = activityResultID.Value
                };
            }
            else if (activityResultID.Value == DeleteResultID)
            {
                db.Requests.Remove(_entity);
                db.Actions.Remove(task);                

                await db.SaveChangesAsync();

                return null;
            }
            else
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }
        }

    }
}
