using Lpp.Dns.DTO.Enums;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class CodeMetricType : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.CodeMetricID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "CodeMetric/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "CodeMetric/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return null; }
            //get { return "CodeMetric/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return null; }
            //get { return "CodeMetric/stratifierview.cshtml"; }
        }

        public string ProjectionEditRelativePath
        {
            get { return null; }
        }

        public string ProjectionViewRelativePath
        {
            get { return null; }
        }


        public string Name
        {
            get { return "Code Metric Types"; }

        }

        public string Description
        {
            get { return "General lookup of Metric Types of the visit."; }
        }

        public string Category
        {
            get
            {
                return "Criteria";
            }
        }


        public object ValueTemplate
        {
            get { return new MetricValues(); }
        }
    }

    public class MetricValues
    {
        public CodeMetric Metric { get; set; }
    }
}