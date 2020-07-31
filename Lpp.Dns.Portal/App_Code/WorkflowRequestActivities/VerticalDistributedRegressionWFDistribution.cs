using Lpp.Workflow.Engine;
using System;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class VerticalDistributedRegressionWFDistribution : BaseDistributedRegressionActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("94E90001-A620-4624-9003-A64F0121D0D7"); }
        }

        public string Name
        {
            get { return "Distribution"; }
        }

        public string Path
        {
            get { return "VerticalDistributedRegressionDistribution"; }
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