using Lpp.Dns.Data;
using System.Data.Entity;
using Lpp.Utilities;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Workflow.DataChecker.Activities
{
    public class ApproveResponse : ActivityBase<Request>
    {
        static readonly Guid ApproveResultID = new Guid("0FEE0001-ED08-48D8-8C0B-A3B600EEF30F");
        static readonly Guid RejectID = new Guid("F1B10001-B0B3-45A9-AAFF-A3B600EEFC49");
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

        public override Guid ID
        {
            get { return DataCheckerWorkflowConfiguration.ApproveResponseID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Results Review";
            }
        }

        public override string Uri
        {
            get { return "/requests/details?ID=" + _entity.ID; }
        }

        public override async Task<Lpp.Workflow.Engine.ValidationResult> Validate(Guid? activityResultID)
        {
            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity);

            if (!permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask))
            {
                return new ValidationResult
                {
                    Success = false,
                    Errors = CommonMessages.RequirePermissionToCloseTask
                };
            }

            return new ValidationResult
            {
                Success = true
            };
        }

        public override async Task<CompletionResult> Complete(string data, Guid? activityResultID)
        {
            if (activityResultID == null)
                throw new NotSupportedException(CommonMessages.ActivityResultIDRequired);

            if (activityResultID.Value == SaveResultID) //Save Metadata
            {
                return new CompletionResult
                {
                    ResultID = SaveResultID
                };
            }
            
            var responseData = JsonConvert.DeserializeObject<ApproveRejectResponseDTO>(data);

            if (activityResultID.Value == ApproveResultID) //Approve
            {
                var Response = await db.Responses.FindAsync(responseData.ResponseID);
                Response.RequestDataMart.Status = DTO.Enums.RoutingStatus.Completed;

                if (await (from rdm in db.RequestDataMarts where rdm.RequestID == _entity.ID select rdm).AllAsync(rdm => rdm.Status == DTO.Enums.RoutingStatus.Completed))
                    _entity.Status = DTO.Enums.RequestStatuses.Complete;
            }
            else if (activityResultID.Value == RejectID) //Reject
            {
                var Response = await db.Responses.FindAsync(responseData.ResponseID);
            }
            else
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }

            var task = PmnTask.GetActiveTaskForRequestActivity(_entity.ID, ID, db);
            if (task != null)
            {
                task.Status = DTO.Enums.TaskStatuses.Complete;
                task.EndOn = DateTime.UtcNow;
            }

            await db.SaveChangesAsync();

            return new CompletionResult
            {
                ResultID = activityResultID.Value
            };
        }

    }
}
