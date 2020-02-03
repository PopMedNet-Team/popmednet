using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lpp.QueryComposer;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class Hispanic : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.HispanicID; }
        }

        public string Name
        {
            get { return ModelTermResources.Hispanic_Name; }
        }

        public string Description
        {
            get { return ModelTermResources.Hispanic_Description; }
        }

        public string Category
        {
            get { return "Demographic"; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "hispanic/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "hispanic/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "hispanic/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "hispanic/stratifierview.cshtml"; }
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
            get { return new HispanicSelectionValues(); }
        }

        public class HispanicSelectionValues
        {

            public Lpp.Dns.DTO.Enums.Hispanic Hispanic { get; set; }
        }
    }
}