using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class SimpleModularProgramWFDistributeRequest : BaseSimpleModularProgramActivity, IVisualWorkflowActivity
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
            get { return "SimpleModularProgramDistributeRequest"; }
        }

    }
}