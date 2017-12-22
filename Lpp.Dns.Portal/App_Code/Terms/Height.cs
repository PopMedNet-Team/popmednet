using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class Height : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.HeightID; }
        }

        public string Name
        {
            get { return ModelTermResources.Height_Name; }
        }

        public string Description
        {
            get { return ModelTermResources.Height_Description; }
        }

        public string Category
        {
            get { return "Criteria"; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "Height/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "Height/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "Height/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "Height/stratifierview.cshtml"; }
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
            get { return new HeightValues(); }
        }

        public class HeightValues
        {

                public double? HeightMin { get; set; }
                public double? HeightMax { get; set; }
        }
    }
}