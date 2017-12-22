using Lpp.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class DataCheckerWFReviewResponses : BaseDataCheckerActivity, IVisualWorkflowActivity
    {
        public Guid WorkflowActivityID
        {
            get { return new Guid("1D0D4993-EA87-4C0D-9226-43F8BB83C952"); }
        }

        public string Name
        {
            get { return "Review Responses"; }
        }

        public string Path
        {
            get { return "DataCheckerApproveResponse"; }
        }
    }
}