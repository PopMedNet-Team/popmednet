using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class ModularProgramWFSpecificationReview : BaseModularProgramActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("948B60F0-8CE5-4B14-9AD6-C50EC37DFC77"); }
        }

        public string Name
        {
            get { return "Specifications Review"; }
        }

        public string Path
        {
            get { return "ModularProgramSpecificationReview"; }
        }
    }
}