using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class SimpleModularProgramWFViewStatusAndResults : BaseSimpleModularProgramActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("D0E659B8-1155-4F44-9728-B4B6EA4D4D55"); }
        }

        public string Name
        {
            get { return "Complete Distribution"; }
        }

        public string Path
        {
            get { return "SimpleModularProgramViewStatusAndResults"; }
        }
        public override Guid? WorkflowID
        {
            get
            {
                return new Guid("931c0001-787c-464d-a90f-a64f00fb23e7");
            }
        }
    }
}