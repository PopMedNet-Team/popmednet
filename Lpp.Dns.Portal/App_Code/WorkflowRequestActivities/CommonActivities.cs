using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lpp.Workflow.Engine;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    public class CommonSubmitDraftReport : BaseQueryComposerActivity, IVisualWorkflowActivity
    {
        public Guid WorkflowActivityID
        {
            get { return new Guid("9173A8E7-27C4-469D-853D-69A78501A522"); }
        }

        public string Name
        {
            get { return "Draft Report"; }
        }

        public string Path
        {
            get { return "CommonSubmitDraftReport"; }
        }
    }

    public class CommonReviewDraftReport : BaseQueryComposerActivity, IVisualWorkflowActivity
    {
        public Guid WorkflowActivityID
        {
            get { return new Guid("2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81"); }
        }

        public string Name
        {
            get { return "Draft Report Review"; }
        }

        public string Path
        {
            get { return "CommonReviewDraftReport"; }
        }
    }

    public class CommonSubmitFinalReport : BaseQueryComposerActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("F888C5D6-B8EB-417C-9DE2-4A96D75F3208"); }
        }

        public string Name
        {
            get { return "Final Report"; }
        }

        public string Path
        {
            get { return "CommonSubmitFinalReport"; }
        }
    }

    public class CommonReviewFinalReport : BaseQueryComposerActivity, IVisualWorkflowActivity
    {

        public Guid WorkflowActivityID
        {
            get { return new Guid("2E7A3263-C87E-47BA-AC35-A78ABF8FE606"); }
        }

        public string Name
        {
            get { return "Final Report Review"; }
        }

        public string Path
        {
            get { return "CommonReviewFinalReport"; }
        }
    }

    public class CommonCompleted : BaseQueryComposerActivity, IVisualWorkflowActivity
    {
        public Guid WorkflowActivityID
        {
            get { return new Guid("9392ACEF-1AF3-407C-B19C-BAE88C389BFC"); }
        }

        public string Name
        {
            get { return "Completed"; }
        }

        public string Path
        {
            get { return "CommonCompleted"; }
        }
    }
}