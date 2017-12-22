using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms.DataChecker
{
    public class DispensingRXSupply : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.DC_DispensingRXSupply; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "datachecker/dispensingrxsupply/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "datachecker/dispensingrxsupply/view.cshtml"; }
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
            get { return "Dispensing RX Supply"; }
        }

        public string Description
        {
            get { return "Dispensing RX Supply"; }
        }

        public string Category
        {
            get { return "Data Checker"; }
        }

        public object ValueTemplate
        {
            get { return new DispensingRXSupplyValues(); }
        }

        public class DispensingRXSupplyValues
        {
            public DispensingRXSupplyValues()
            {
                RXSupply = new List<int>();
            }

            public IEnumerable<int> RXSupply { get; set; }
        }
    }
}