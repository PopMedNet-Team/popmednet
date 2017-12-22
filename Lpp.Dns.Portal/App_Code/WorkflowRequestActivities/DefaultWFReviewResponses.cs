using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class DefaultWFReviewResponses : BaseQueryComposerActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("ACBA0001-0CE4-4C00-8DD3-A3B5013A3344"); }
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