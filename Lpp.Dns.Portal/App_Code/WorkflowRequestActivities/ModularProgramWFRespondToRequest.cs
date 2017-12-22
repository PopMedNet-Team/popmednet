using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class ModularProgramWFRespondToRequest : BaseModularProgramActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("D51D0D4F-41F7-4208-8722-6D71B23DE2F9"); }
        }

        public string Name
        {
            get { return "Respond to Request"; }
        }

        public string Path
        {
            get { return "ModularProgramRespondToRequest"; }
        }
    }
}