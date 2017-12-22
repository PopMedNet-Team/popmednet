using Lpp.Dns.DTO;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class DrugName : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.DrugNameID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "drugname/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "drugname/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "drugname/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "drugname/stratifierview.cshtml"; }
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
            get { return "Drug Name"; }

        }

        public string Description
        {
            get { return "General lookup of Drug Names."; }
        }

        public string Category
        {
            get
            {
                return "Drug";
            }
        }


        public object ValueTemplate
        {
            get { return new DrugNameValues(); }
        }
    }

    public class DrugNameValues
    {
        public DrugNameValues()
        {
            this.CodeValues = new List<CodeSelectorValueDTO>();
            this.Codes = new List<string>();
        }
        [Obsolete]
        public IEnumerable<string> Codes { get; set; }
        public IEnumerable<CodeSelectorValueDTO> CodeValues { get; set; }
    }
}