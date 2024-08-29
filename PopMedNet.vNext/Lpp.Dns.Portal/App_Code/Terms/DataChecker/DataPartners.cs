using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms.DataChecker
{
    public class DataPartners : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.DC_DataPartners; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "datachecker/datapartners/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "datachecker/datapartners/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "datachecker/datapartners/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "datachecker/datapartners/stratifierview.cshtml"; }
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
            get { return "Data Partners"; }
        }

        public string Description
        {
            get { return "Data Partners"; }
        }

        public string Category
        {
            get { return "Data Checker"; }
        }

        public object ValueTemplate
        {
            get { return new DataPartnersValues(); }
        }
    }

    public class DataPartnersValues
    {
        public DataPartnersValues()
        {
            this.DataPartnersValue = new List<string>();
        }
        
        public IEnumerable<string> DataPartnersValue { get; set; }
    }
}