using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms.DataChecker
{
    public class Ethnicity : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.DC_Ethnicity; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "datachecker/ethnicity/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "datachecker/ethnicity/view.cshtml"; }
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
            get { return "Ethnicity"; }
        }

        public string Description
        {
            get { return "Ethnicity"; }
        }

        public string Category
        {
            get { return "Data Checker"; }
        }

        public object ValueTemplate
        {
            get { return new EthnicityValues(); }
        }

        public class EthnicityValues
        {
            public EthnicityValues()
            {
                EthnicityValue = new List<int>();
            }

            public IEnumerable<int> EthnicityValue { get; set; }
        }
    }
}