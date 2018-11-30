using Lpp.Dns.Data;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Workflow.ModularProgram.Activities
{
    public class TerminateRequest: IActivity<DataContext, Request>
    {
        Workflow<DataContext, Request> Workflow;

        public void Initialize(Lpp.Workflow.Engine.Workflow<DataContext, Request> workflow)
        {
            this.Workflow = workflow;
        }

        public Guid ID
        {
            get { return SimpleModularProgramWorkflowConfiguration.TerminateRequestID; }
        }

        public string Uri
        {
            get { return null; }
        }

        public async Task<Lpp.Workflow.Engine.ValidationResult> Validate(Guid? activityResultID)
        {

            if (!(await Workflow.DataContext.GetGrantedWorkflowActivityPermissionsForRequestAsync(Workflow.Identity, Workflow.Entity, Lpp.Dns.DTO.Security.PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow)).Any())
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToTerminateWorkflow };
            }

            return new ValidationResult { Success = true };
        }

        public async Task<Lpp.Workflow.Engine.CompletionResult> Complete(string data, Guid? activityResultID)
        {

            //TODO: need to somehow know if it should be canceled or not, this is getting called to end the workflow as well

            Workflow.Entity.CancelledByID = Workflow.Identity.ID;
            Workflow.Entity.CancelledOn = DateTime.UtcNow;

            await Workflow.DataContext.SaveChangesAsync();

            return null;
        }

        public Task Start(string comment)
        {
            return Task.Run(() => { });
        }
    
    }
}
