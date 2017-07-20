using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class ModularProgramWFReviewRequest : BaseModularProgramActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("A96FBAD0-8FD8-4D10-8891-D749A71912F8"); }
        }

        public string Name
        {
            get { return "Request Form Review"; }
        }

        public string Path
        {
            get { return "ModularProgramReviewRequest"; }
        }
    }
}