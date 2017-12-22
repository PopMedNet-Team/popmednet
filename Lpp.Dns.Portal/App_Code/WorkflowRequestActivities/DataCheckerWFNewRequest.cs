using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class DataCheckerWFNewRequest : BaseDataCheckerActivity, IVisualWorkflowActivity
    {
        public Guid WorkflowActivityID
        {
            get { return new Guid("11383C00-C270-4A46-97D2-5B1AC527B7F8"); }
        }

        public string Name
        {
            get { return "New Request"; }
        }

        public string Path
        {
            get { return "DataCheckerCreateRequest"; }
        }
    }
}