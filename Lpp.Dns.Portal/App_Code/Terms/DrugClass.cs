using Lpp.Dns.DTO;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class DrugClass : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.DrugClassID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "drugclass/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "drugclass/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "drugclass/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "drugclass/stratifierview.cshtml"; }
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
            get { return "Drug Class"; }

        }

        public string Description
        {
            get { return "General lookup of Drug Classes."; }
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
            get { return new DrugClassValues(); }
        }
    }

    public class DrugClassValues
    {
        public DrugClassValues()
        {
            this.CodeValues = new List<CodeSelectorValueDTO>();
            this.Codes = new List<string>();
        }
        [Obsolete]
        public IEnumerable<string> Codes { get; set; }
        public IEnumerable<CodeSelectorValueDTO> CodeValues { get; set; }
    }
}