using Lpp.Workflow.Engine;
using System;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class VerticalDistributedRegressionWFCompleted : BaseDistributedRegressionActivity, IVisualWorkflowActivity
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
            get { return "VerticalDistributedRegressionCompleted"; }
        }
        public override Guid? WorkflowID
        {
            get
            {
                return new Guid("047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A");
            }
        }
    }
}