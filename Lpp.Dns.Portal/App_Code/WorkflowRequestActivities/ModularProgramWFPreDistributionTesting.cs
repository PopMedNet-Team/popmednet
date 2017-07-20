using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class ModularProgramWFPreDistributionTesting : BaseModularProgramActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7"); }
        }

        public string Name
        {
            get { return "Pre-Distribution Testing"; }
        }

        public string Path
        {
            get { return "ModularProgramPreDistributionTesting"; }
        }
    }
}