using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class SummaryQueryWFDraftRequest : BaseSummaryQueryActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("197AF4BA-F079-48DD-9E7C-C7BE7F8DC896"); }
        }

        public string Name
        {
            get { return "Request Form"; }
        }

        public string Path
        {
            get { return "SummaryQueryDraftRequest"; }
        }
    }
}