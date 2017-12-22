using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class SummaryQueryWFReviewResponses : BaseSummaryQueryActivity, IVisualWorkflowActivity
    {
        public Guid WorkflowActivityID
        {
            get { return new Guid("6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B"); }
        }

        public string Name
        {
            get { return "Complete Distribution"; }
        }

        public string Path
        {
            get { return "CommonListRoutings"; }
        }
    }
}