using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class DistributedRegressionWFCompleted : BaseDistributedRegressionActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("9392ACEF-1AF3-407C-B19C-BAE88C389BFC"); }
        }

        public string Name
        {
            get { return "Distribution"; }
        }

        public string Path
        {
            get { return "DistributedRegressionCompleted"; }
        }
        public override Guid? WorkflowID
        {
            get
            {
                return new Guid("E9656288-33FF-4D1F-BA77-C82EB0BF0192");
            }
        }
    }
}