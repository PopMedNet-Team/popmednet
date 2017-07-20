using Lpp.Dns.Data;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Workflow.Default.Activities
{
    public class TerminateRequest : IActivity<DataContext, Request>
    {
        Workflow<DataContext, Request> Workflow;

        public void Initialize(Lpp.Workflow.Engine.Workflow<DataContext, Request> workflow)
        {
            this.Workflow = workflow;
        }

        public Guid ID
        {
            get { return DefaultWorkflowConfiguration.TerminateRequestID; }
        }

        public string Uri
        {
            get { return "requests/details?ID=" + this.Workflow.Entity.ID; }
        }

        public async Task<Lpp.Workflow.Engine.ValidationResult> Validate(Guid? activityResultID)
        {
            if (! (await this.Workflow.DataContext.GetGrantedPermissionsForWorkflowActivityAsync(Workflow.Identity, Workflow.Entity.ProjectID, DefaultWorkflowConfiguration.TerminateRequestID, Workflow.Entity.RequestTypeID, Lpp.Dns.DTO.Security.PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow)).Any())
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

        public async Task<Lpp.Workflow.Engine.CompletionResult> Complete(string data, Guid? activityResultID)
        {
            Workflow.Entity.CancelledByID = Workflow.Identity.ID;
            Workflow.Entity.CancelledOn = DateTime.UtcNow;

            await Workflow.DataContext.SaveChangesAsync();

            return null;
        }

        public async Task Start(string comment)
        {

        }
    }
}
