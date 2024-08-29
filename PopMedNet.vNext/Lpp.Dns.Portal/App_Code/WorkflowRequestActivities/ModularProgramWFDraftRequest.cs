using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class ModularProgramWFDraftRequest : BaseModularProgramActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("0321E17F-AA1F-4B23-A145-85B159E74F0F"); }
        }

        public string Name
        {
            get { return "Request Form"; }
        }

        public string Path
        {
            get { return "ModularProgramDraftRequest"; }
        }
    }
}