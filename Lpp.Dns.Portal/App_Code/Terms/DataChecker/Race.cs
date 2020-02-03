using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms.DataChecker
{
    public class Race : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.DC_Race; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "datachecker/race/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "datachecker/race/view.cshtml"; }
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
            get { return "Race"; }
        }

        public string Description
        {
            get { return "Race"; }
        }

        public string Category
        {
            get { return "Data Checker"; }
        }

        public object ValueTemplate
        {
            get { return new RaceValues(); }
        }

        public class RaceValues
        {
            public RaceValues()
            {
                Races = new List<int>();
            }

            public IEnumerable<int> Races { get; set; }
        }
    }
}