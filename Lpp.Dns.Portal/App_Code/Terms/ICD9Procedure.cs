using Lpp.Dns.DTO;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class ICD9Procedure3digit : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.ICD9ProcedureCodes3digitID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "icd9procedure/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "icd9procedure/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "icd9procedure/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "icd9procedure/stratifierview.cshtml"; }
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
            get { return "ICD-9 Procedures (3 digit)"; }

        }

        public string Description
        {
            get { return "Filter and validate based on 3 digit ICD-9 Procedures Codes"; }
        }

        public string Category
        {
            get
            {
                return "ICD 9";
            }
        }


        public object ValueTemplate
        {
            get { return new ICD9ProcedureValues(); }
        }
    }

    public class ICD9ProcedureValues {
        public ICD9ProcedureValues()
        {
            this.CodeValues = new List<CodeSelectorValueDTO>();
            this.Codes = new List<string>();
        }
        [Obsolete]
        public IEnumerable<string> Codes { get; set; }
        public IEnumerable<CodeSelectorValueDTO> CodeValues { get; set; }
    }
}