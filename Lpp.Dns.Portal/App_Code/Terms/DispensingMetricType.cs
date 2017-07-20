using Lpp.QueryComposer;
using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class DispensingMetricType : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.DispensingMetricID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "DispensingMetric/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "DispensingMetric/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return null; }
            //get { return "DispensingMetric/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return null; }
            //get { return "DispensingMetric/stratifierview.cshtml"; }
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
            get { return "Dispensing Metric Types"; }

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
            get { return new DispensingMetricValues(); }
        }
    }

    public class DispensingMetricValues
    {
        public DispensingMetric Metric { get; set; }
    }
}