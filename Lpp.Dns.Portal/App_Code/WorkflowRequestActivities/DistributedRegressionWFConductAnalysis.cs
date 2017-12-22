using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class DistributedRegressionWFConductAnalysis : BaseDistributedRegressionActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("370646FC-7A47-43B5-A4B3-659F90A188A9"); }
        }

        public string Name
        {
            get { return "Conduct Analysis"; }
        }

        public string Path
        {
            get { return "DistributedRegressionConductAnalysis"; }
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