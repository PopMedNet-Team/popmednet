using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class DefaultWFReview : BaseQueryComposerActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("73740001-A942-47B0-BF6E-A3B600E7D9EC"); }
        }

        public string Name
        {
            get { return "Request Review"; }
        }

        public string Path
        {
            get { return "DefaultRequestReview"; }
        }
    }
}