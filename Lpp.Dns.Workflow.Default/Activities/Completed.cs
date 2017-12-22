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
    class Completed : ActivityBase<Request>
    {
        //to save Metadata
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

        public override Guid ID
        {
            get { return DefaultWorkflowConfiguration.CompletedID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Completed";
            }
        }

        public override string Uri
        {
            get { return "requests/details?ID=" + _entity.ID; }
        }

        public override async Task<Lpp.Workflow.Engine.ValidationResult> Validate(Guid? activityResultID)
        {
            return new ValidationResult
            {
                Success = true
            };
        }

        public override async Task<Lpp.Workflow.Engine.CompletionResult> Complete(string data, Guid? activityResultID)
        {
            if (activityResultID.Value == SaveResultID)
            {
                return new CompletionResult
                {
                    ResultID = SaveResultID
                };
            }

            return new CompletionResult
            {
                ResultID = activityResultID.Value
            };
        }

        public override async Task Start(string comment)
        {

        }
    }
}
