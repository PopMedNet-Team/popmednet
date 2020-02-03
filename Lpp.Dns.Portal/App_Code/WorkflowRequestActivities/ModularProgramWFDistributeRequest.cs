using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class ModularProgramWFDistributeRequest : BaseModularProgramActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("E6CCD61B-81C4-4217-A958-ADAFB5EE5554"); }
        }

        public string Name
        {
            get { return "Distribution"; }
        }

        public string Path
        {
            get { return "ModularProgramDistributeRequest"; }
        }
    }
}