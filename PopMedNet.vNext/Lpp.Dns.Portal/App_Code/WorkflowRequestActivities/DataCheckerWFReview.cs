using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class DataCheckerWFReview : BaseDataCheckerActivity, IVisualWorkflowActivity
    {
        public Guid WorkflowActivityID
        {
            get { return new Guid("3FFBCA99-5801-4045-9FB4-072136A845FC"); }
        }

        public string Name
        {
            get { return "Review Request"; }
        }

        public string Path
        {
            get { return "DataCheckerRequestReview"; }
        }
    }
}