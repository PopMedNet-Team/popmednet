using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class Ethnicity : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.EthnicityID; }
        }

        public string Name
        {
            get { return ModelTermResources.Ethnicity_Name; }
        }

        public string Description
        {
            get { return ModelTermResources.Ethnicity_Description; }
        }

        public string Category
        {
            get { return "Demographic"; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "ethnicity/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "ethnicity/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "ethnicity/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "ethnicity/stratifierview.cshtml"; }
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
            get { return new EthnicitySelectionValues(); }
        }

        public class EthnicitySelectionValues
        {
            public EthnicitySelectionValues()
            {
                this.Ethnicities = new List<Lpp.Dns.DTO.Enums.Ethnicities>();
            }

            public IEnumerable<Lpp.Dns.DTO.Enums.Ethnicities> Ethnicities { get; set; }
        }
    }
}