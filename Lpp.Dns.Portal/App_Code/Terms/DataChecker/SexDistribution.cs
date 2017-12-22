using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms.DataChecker
{
    public class SexDistribution : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.DC_SexDistribution; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "datachecker/sexdistribution/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "datachecker/sexdistribution/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            //get { return "datachecker/sexdistribution/editstratifierview.cshtml"; }
            get { return null; }
        }

        public string StratifierViewRelativePath
        {
            //get { return "datachecker/sexdistribution/stratifierview.cshtml"; }
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
            get { return "Sex Distribution"; }
        }

        public string Description
        {
            get { return "Sex Distribution"; }
        }

        public string Category
        {
            get { return "Data Checker"; }
        }

        public object ValueTemplate
        {
            get { return new SexDistributionValues(); }
        }

        public class SexDistributionValues
        {
            public SexDistributionValues()
            {
                SexDistributions = new List<int>();
            }

            public IEnumerable<int> SexDistributions { get; set; }
        }
    }
}