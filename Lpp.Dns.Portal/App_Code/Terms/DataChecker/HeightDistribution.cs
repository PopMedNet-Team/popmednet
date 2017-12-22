using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms.DataChecker
{
    public class HeightDistribution : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.DC_HeightDistribution; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "datachecker/heightdistribution/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "datachecker/heightdistribution/view.cshtml"; }
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

        public string Name
        {
            get { return "Height Distribution"; }
        }

        public string Description
        {
            get { return "Height Distribution"; }
        }

        public string Category
        {
            get { return "Data Checker"; }
        }

        public object ValueTemplate
        {
            get { return new HeightDistributionValues(); }
        }

        public class HeightDistributionValues
        {
            public HeightDistributionValues()
            {
                HeightDistributions = new List<int>();
            }

            public IEnumerable<int> HeightDistributions { get; set; }
        }
    }
}