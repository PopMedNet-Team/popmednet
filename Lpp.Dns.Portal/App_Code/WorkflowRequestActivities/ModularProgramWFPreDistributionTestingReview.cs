using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class ModularProgramWFPreDistributionTestingReview : BaseModularProgramActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("EA69E5ED-6029-47E8-9B45-F0F00B07FDC2"); }
        }

        public string Name
        {
            get { return "Pre-Distribution Testing Review"; }
        }

        public string Path
        {
            get { return "ModularProgramPreDistributionTestingReview"; }
        }
    }
}