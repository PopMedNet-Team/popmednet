using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class DefaultWFNewRequest : BaseQueryComposerActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343"); }
        }

        public string Name
        {
            get { return "New Request"; }
        }

        public string Path
        {
            get { return "DefaultCreateRequest"; }
        }
    }
}