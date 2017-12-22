using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class DefaultWFTerminateRequest : BaseQueryComposerActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("CC2E0001-9B99-4C67-8DED-A3B600E1C696"); }
        }

        public string Name
        {
            get { return "Terminate Request"; }
        }

        public string Path
        {
            get { return "DefaultTerminateRequest"; }
        }
    }
}