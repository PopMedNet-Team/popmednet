using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class ModularProgramWFReviewDataMartResponses : BaseModularProgramActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("7B4EB88B-1295-45B9-AE19-5BC45E98C985"); }
        }

        public string Name
        {
            get { return "Review DataMart Responses"; }
        }

        public string Path
        {
            get { return "ModularProgramReviewDataMartResponses"; }
        }
    }
}