using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class VitalMeasureDate : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.VitalsMeasureDateID; }
        }

        public string Name
        {
            get { return ModelTermResources.VitalsMeasureDate_Name; }
        }

        public string Description
        {
            get { return ModelTermResources.VitalsMeasureDate_Description; }
        }

        public string Category
        {
            get { return "Criteria"; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "vitalmeasuredate/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "vitalmeasuredate/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return null; }
        }

        public string StratifierViewRelativePath
        {
            get { return null; }
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
            get { return new VitalMeasureDateValues(); }
        }
    }

    public class VitalMeasureDateValues
    {
        public DateTimeOffset? Start { get; set; }

        public DateTimeOffset? End { get; set; }
    }
}