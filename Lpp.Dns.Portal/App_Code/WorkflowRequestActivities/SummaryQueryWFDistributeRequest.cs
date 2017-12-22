using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class SummaryQueryWFDataMartResponse : BaseSummaryQueryActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("752B83D7-2190-49DF-9BAE-983A7880A899"); }
        }

        public string Name
        {
            get { return "Distribution"; }
        }

        public string Path
        {
            get { return "SummaryQueryDistributeRequest"; }
        }
    }
}