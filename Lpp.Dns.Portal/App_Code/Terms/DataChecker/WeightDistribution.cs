using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms.DataChecker
{
    public class WeightDistribution : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.DC_WeightDistribution; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "datachecker/weightdistribution/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "datachecker/weightdistribution/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            //get { return "datachecker/weightdistribution/editstratifierview.cshtml"; }
            get { return null; }
        }

        public string StratifierViewRelativePath
        {
            //get { return "datachecker/weightdistribution/stratifierview.cshtml"; }
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

        public string Name
        {
            get { return "Weight Distribution"; }
        }

        public string Description
        {
            get { return "Weight Distribution"; }
        }

        public string Category
        {
            get { return "Data Checker"; }
        }

        public object ValueTemplate
        {
            get { return new WeightDistributionValues(); }
        }

        public class WeightDistributionValues
        {
            public WeightDistributionValues()
            {
                WeightDistributions = new List<int>();
            }

            public IEnumerable<int> WeightDistributions { get; set; }
        }
    }
}