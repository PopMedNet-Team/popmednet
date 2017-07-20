using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms.DataChecker
{
    public class Encounter : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.DC_Encounter; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "datachecker/encounter/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "datachecker/encounter/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            //get { return "datachecker/encounter/editstratifierview.cshtml"; }
            get { return null; }
        }

        public string StratifierViewRelativePath
        {
            //get { return "datachecker/encounter/stratifierview.cshtml"; }
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
            get { return "Encounter"; }
        }

        public string Description
        {
            get { return "Encounter"; }
        }

        public string Category
        {
            get { return "Data Checker"; }
        }

        public object ValueTemplate
        {
            get { return new EncounterValues(); }
        }

        public class EncounterValues
        {
            public EncounterValues()
            {
                Encounters = new List<string>();
            }

            public IEnumerable<string> Encounters { get; set; }
        }
    }
}