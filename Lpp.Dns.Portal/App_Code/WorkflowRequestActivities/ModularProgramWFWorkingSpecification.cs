using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class ModularProgramWFWorkingSpecification : BaseModularProgramActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("31C60BB1-2F6A-423B-A7B7-B52626FD9E97"); }
        }

        public string Name
        {
            get { return "Working Specifications"; }
        }

        public string Path
        {
            get { return "ModularProgramWorkingSpecification"; }
        }
    }
}