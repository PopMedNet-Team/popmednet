using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class Weight : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.WeightID; }
        }

        public string Name
        {
            get { return ModelTermResources.Weight_Name; }
        }

        public string Description
        {
            get { return ModelTermResources.Weight_Description; }
        }

        public string Category
        {
            get { return "Criteria"; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "Weight/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "Weight/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "Weight/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "Weight/stratifierview.cshtml"; }
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
            get { return new WeightValues(); }
        }

        public class WeightValues
        {

            public double? WeightMin { get; set; }
            public double? WeightMax { get; set; }
        }
    }
}