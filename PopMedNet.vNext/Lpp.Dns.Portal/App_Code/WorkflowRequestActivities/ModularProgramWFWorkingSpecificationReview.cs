using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class ModularProgramWFWorkingSpecificationReview : BaseModularProgramActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("C8891CFD-80BF-4F71-90DE-6748BF71566C"); }
        }

        public string Name
        {
            get { return "Working Specification Review"; }
        }

        public string Path
        {
            get { return "ModularProgramWorkingSpecificationReview"; }
        }
    }
}