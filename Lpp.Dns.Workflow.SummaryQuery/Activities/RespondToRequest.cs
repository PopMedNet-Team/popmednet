using Lpp.Dns.Data;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Workflow.SummaryQuery.Activities
{
    //This is done by the dmc, hence why no logic here.
    public class RespondToRequest : ActivityBase<Request>
    {
        static readonly Guid UploadResponseResultID = new Guid("668EE9C7-4930-423E-AA9E-150B646121F4");
        static readonly Guid RejectResponseResultID = new Guid("D0A0924F-F4B5-43BF-89A6-C7F32E764735");
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

        public override Guid ID
        {
            get { return SummaryQueryWorkflowConfiguration.RespondToRequestID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Results Pending Review";
            }
        }

        public override string Uri
        {
            get { return "/requests/details?ID=" + _entity.ID; }
        }

        public override async Task<ValidationResult> Validate(Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultIDRequired };
            }

            if (activityResultID.Value != UploadResponseResultID &&
                activityResultID.Value != RejectResponseResultID &&
                activityResultID.Value != SaveResultID)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultNotSupported };
            }

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, Lpp.Dns.DTO.Security.PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask);
            if (!permissions.Any())
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
            var task = PmnTask.GetActiveTaskForRequestActivity(_entity.ID, ID, db);

            if (activityResultID.Value == SaveResultID)
            {
                return new CompletionResult
                {
                    ResultID = SaveResultID
                };
            }
            
            if (task != null)
            {
                task.Status = DTO.Enums.TaskStatuses.Complete;
                task.EndOn = DateTime.UtcNow;
                await db.SaveChangesAsync();
            }

            return null;
        }
    }
}
