using Lpp.Workflow.Engine;
using System;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class VerticalDistributedRegressionWFCompleteDistribution : BaseDistributedRegressionActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("D0E659B8-1155-4F44-9728-B4B6EA4D4D55"); }
        }

        public string Name
        {
            get { return "Complete Distribution"; }
        }

        public string Path
        {
            get { return "VerticalDistributedRegressionCompleteDistribution"; }
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