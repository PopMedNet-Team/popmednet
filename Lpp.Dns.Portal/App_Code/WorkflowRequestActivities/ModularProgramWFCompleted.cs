using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class ModularProgramWFCompleted : BaseModularProgramActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("9392ACEF-1AF3-407C-B19C-BAE88C389BFC"); }
        }

        public string Name
        {
            get { return "Completed"; }
        }

        public string Path
        {
            get { return "ModularProgramCompleted"; }
        }

        public override Guid? WorkflowID
        {
            get
            {
                return new Guid("5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D");
            }
        }
    }
}