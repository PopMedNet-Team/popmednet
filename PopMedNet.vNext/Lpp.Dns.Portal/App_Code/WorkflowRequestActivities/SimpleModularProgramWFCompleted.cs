using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class SimpleModularProgramWFCompleted : BaseModularProgramActivity, IVisualWorkflowActivity
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
            get { return "SimpleModularProgramCompleted"; }
        }

        public override Guid? WorkflowID
        {
            get
            {
                return new Guid("931C0001-787C-464D-A90F-A64F00FB23E7");
            }
        }
    }
}