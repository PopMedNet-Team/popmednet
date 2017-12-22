using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class SummaryQueryWFRequestReview : BaseSummaryQueryActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("CC1BCADD-4487-47C7-BDCA-1010F2C68FE0"); }
        }

        public string Name
        {
            get { return "Request Form Review"; }
        }

        public string Path
        {
            get { return "SummaryQueryRequestReview"; }
        }
    }
}