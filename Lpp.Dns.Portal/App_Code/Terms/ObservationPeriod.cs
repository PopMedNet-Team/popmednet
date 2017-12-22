using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class ObservationPeriod : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.ObservationPeriodID; }
        }

        public string Name
        {
            get { return ModelTermResources.ObservationPeriod_Name; }
        }

        public string Description
        {
            get { return ModelTermResources.ObservationPeriod_Description; }
        }

        public string Category
        {
            get { return "Criteria"; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "observationperiod/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "observationperiod/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "observationperiod/EditStratifierView.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "observationperiod/StratifierView.cshtml"; }
        }

        public string ProjectionEditRelativePath
        {
            get { return null; }
        }

        public string ProjectionViewRelativePath
        {
            get { return null; }
        }

        public object ValueTemplate
        {
            get { return new ObservationPeriodValues(); }
        }
    }

    public class ObservationPeriodValues
    {
        public DateTimeOffset? Start {get; set;}

        public DateTimeOffset? End { get; set; }
    }
}