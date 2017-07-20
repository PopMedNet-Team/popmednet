using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lpp.QueryComposer;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class Race : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.RaceID; }
        }

        public string Name
        {
            get { return ModelTermResources.Race_Name; }
        }

        public string Description
        {
            get { return ModelTermResources.Race_Description; }
        }

        public string Category
        {
            get { return "Demographic"; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "race/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "race/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "race/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "race/stratifierview.cshtml"; }
        }

        public string ProjectionEditRelativePath
        {
            get { return null; }
        }

        public string ProjectionViewRelativePath
        {
            get { return null; }
        }

        public object ValueTemplate
        {
            get { return new RaceSelectionValues(); }
        }

        public class RaceSelectionValues
        {
            public RaceSelectionValues()
            {
                this.Race = new List<Lpp.Dns.DTO.Enums.Race>();
            }

            public IEnumerable<Lpp.Dns.DTO.Enums.Race> Race { get; set; }
        }
    }
}