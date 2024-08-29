using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class TobaccoUse : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.TobaccoUseID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "tobaccouse/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "tobaccouse/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "tobaccouse/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "tobaccouse/stratifierview.cshtml"; }
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
            get { return "Tobacco Use"; }

        }

        public string Description
        {
            get { return "Filter based on Tobacco usage"; }
        }

        public string Category
        {
            get
            {
                return "Demographic";
            }
        }


        public object ValueTemplate
        {
            get { return new TobaccoUseValues(); }
        }
    }

    public class TobaccoUseValues
    {
        public TobaccoUseValues()
        {
            this.TobaccoUses = new List<Lpp.Dns.DTO.Enums.TobaccoUses>();
        }

        public IEnumerable<Lpp.Dns.DTO.Enums.TobaccoUses> TobaccoUses { get; set; }
    }
}