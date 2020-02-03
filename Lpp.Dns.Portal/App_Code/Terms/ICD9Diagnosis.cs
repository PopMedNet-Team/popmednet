using Lpp.Dns.DTO;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class ICD9Diagnosis3digit : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.ICD9DiagnosisCodes3digitID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "icd9diagnosis/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "icd9diagnosis/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "icd9diagnosis/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "icd9diagnosis/stratifierview.cshtml"; }
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
            get { return "ICD-9 Diagnosis (3 digit)"; }

        }

        public string Description
        {
            get { return "Filter and validate based on 3 digit ICD-9 Diagnosis Codes"; }
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
            get { return new ICD9DiagnosisValues(); }
        }
    }

    public class ICD9DiagnosisValues {
        public ICD9DiagnosisValues()
        {
            this.CodeValues = new List<CodeSelectorValueDTO>();
            this.Codes = new List<string>();
        }
        [Obsolete]
        public IEnumerable<string> Codes { get; set; }
        public IEnumerable<CodeSelectorValueDTO> CodeValues { get; set; }
    }
}