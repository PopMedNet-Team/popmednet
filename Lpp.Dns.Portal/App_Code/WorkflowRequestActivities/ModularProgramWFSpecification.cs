using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class ModularProgramWFSpecification : BaseModularProgramActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("C3B13067-3B9D-41E4-8D4A-7114A6E81930"); }
        }

        public string Name
        {
            get { return "Specifications"; }
        }

        public string Path
        {
            get { return "ModularProgramSpecification"; }
        }
    }
}