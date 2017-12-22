using Lpp.Dns.DTO;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class HCPCS : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.HCPCSProcedureCodesID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "hcpcs/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "hcpcs/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "hcpcs/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "hcpcs/stratifierview.cshtml"; }
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
            get { return "HCPCS"; }

        }

        public string Description
        {
            get { return "Filter and validate based on HCPCS Procedures"; }
        }

        public string Category
        {
            get
            {
                return null;
            }
        }


        public object ValueTemplate
        {
            get { return new HCPCSValues(); }
        }
    }

    public class HCPCSValues
    {
        public HCPCSValues()
        {
            this.CodeValues = new List<CodeSelectorValueDTO>();
            this.Codes = new List<string>();
        }
        [Obsolete]
        public IEnumerable<string> Codes { get; set; }
        public IEnumerable<CodeSelectorValueDTO> CodeValues { get; set; }
    }
}