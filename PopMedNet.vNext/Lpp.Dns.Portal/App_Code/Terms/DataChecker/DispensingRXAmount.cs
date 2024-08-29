using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms.DataChecker
{
    public class DispensingRXAmount : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.DC_DispensingRXAmount; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "datachecker/dispensingrxamount/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "datachecker/dispensingrxamount/view.cshtml"; }
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
            get { return "Dispensing RX Amount"; }
        }

        public string Description
        {
            get { return "Dispensing RX Amount"; }
        }

        public string Category
        {
            get { return "Data Checker"; }
        }

        public object ValueTemplate
        {
            get { return new DispensingRXAmountValues(); }
        }

        public class DispensingRXAmountValues
        {
            public DispensingRXAmountValues()
            {
                RXAmounts = new List<int>();
            }

            public IEnumerable<int> RXAmounts { get; set; }
        }
    }
}