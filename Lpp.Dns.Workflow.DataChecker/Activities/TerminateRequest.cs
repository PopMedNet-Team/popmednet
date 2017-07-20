using Lpp.Dns.Data;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Workflow.DataChecker.Activities
{
    public class TerminateRequest : ActivityBase<Request>
    {
        public override Guid ID
        {
            get { return DataCheckerWorkflowConfiguration.TerminateRequestID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Terminated";
            }
        }

        public override string Uri
        {
            get { return null; }
        }

        public override async Task<Lpp.Workflow.Engine.ValidationResult> Validate(Guid? activityResultID)
        {
            if (!(await _workflow.DataContext.GetGrantedPermissionsForWorkflowActivityAsync(_workflow.Identity, _entity.ProjectID, DataCheckerWorkflowConfiguration.TerminateRequestID, _entity.RequestTypeID, Lpp.Dns.DTO.Security.PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow)).Any())
            {
                return new ValidationResult
                {
                    Success = false,
                    Errors = CommonMessages.RequirePermissionToTerminateWorkflow
                };
            }

            return new ValidationResult
            {
                Success = true
            };
        }

        public override async Task<Lpp.Workflow.Engine.CompletionResult> Complete(string data, Guid? activityResultID)
        {
            _entity.CancelledByID = _workflow.Identity.ID;
            _entity.CancelledOn = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return null;
        }

        public override async Task Start(string comment)
        {

        }
    }
}
