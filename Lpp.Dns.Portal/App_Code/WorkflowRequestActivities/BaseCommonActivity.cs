using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lpp.Workflow.Engine;

namespace Lpp.Dns.Portal.App_Code.WorkflowRequestActivities
{
    // These base classes implement the Overview Path method for the IVisualWorkflowActivity to provide a
    // common location for the definition of the UI to be used to display for both QueryComposer and non-QueryComposer requests

    public class BaseDataCheckerActivity
    {
        public string OverviewPath
        {
            get { return "QueryComposer/Views/View.cshtml"; }
        }

        public virtual Guid? WorkflowID
        {
            get { return null; }
        }
    }

    public class BaseQueryComposerActivity
    {
        public string OverviewPath
        {
            get { return "QueryComposer/Views/View.cshtml"; }
        }

        public virtual Guid? WorkflowID
        {
            get { return null; }
        }
    }

    public class BaseModularProgramActivity
    {
        public string OverviewPath
        {
            get { return "QueryComposer/Views/View.cshtml"; }
        }
        public virtual Guid? WorkflowID
        {
            get { return null; }
        }
    }
    public class BaseSimpleModularProgramActivity
    {
        public string OverviewPath
        {
            get { return "QueryComposer/Views/View.cshtml"; }
        }
        public virtual Guid? WorkflowID
        {
            get { return null; }
        }
    }

    public class BaseSummaryQueryActivity
    {
        public string OverviewPath
        {
            get { return "QueryComposer/Views/View.cshtml"; }
        }
        public virtual Guid? WorkflowID
        {
            get { return null; }
        }
    }
    public class BaseDistributedRegressionActivity
    {
        public string OverviewPath
        {
            get { return "QueryComposer/Views/View.cshtml"; }
        }
        public virtual Guid? WorkflowID
        {
            get { return null; }
        }
    }
}