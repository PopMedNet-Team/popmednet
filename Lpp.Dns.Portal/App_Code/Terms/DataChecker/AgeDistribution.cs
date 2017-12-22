using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms.DataChecker
{
    public class AgeDistribution : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.DC_AgeDistribution; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "datachecker/agedistribution/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "datachecker/agedistribution/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            //get { return "datachecker/editstratifierview/edit.cshtml"; }
            get { return null; }
        }

        public string StratifierViewRelativePath
        {
            //get { return "datachecker/stratifierview/edit.cshtml"; }
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
            get { return "Age Distribution"; }
        }

        public string Description
        {
            get { return "Age Distribution"; }
        }

        public string Category
        {
            get { return "Data Checker"; }
        }

        public object ValueTemplate
        {
            get { return new AgeDistributionValues(); }
        }

        public class AgeDistributionValues
        {
            public AgeDistributionValues()
            {
                AgeDistributionValue = new List<int>();
            }

            public IEnumerable<int> AgeDistributionValue { get; set; }
        }

    }
}