using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms.DataChecker
{
    public class NDCCodes : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.DC_NDCCodes; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "datachecker/ndccodes/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "datachecker/ndccodes/view.cshtml"; }
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
            get { return "NDC Codes"; }
        }

        public string Description
        {
            get { return "NDC Codes"; }
        }

        public string Category
        {
            get { return "Data Checker"; }
        }

        public object ValueTemplate
        {
            get { return new NDC_Codes(); }
        }

        public class NDC_Codes
        {
            public NDC_Codes()
            {
                CodeValues = string.Empty;
                SearchMethodType = DTO.Enums.TextSearchMethodType.ExactMatch;
            }

            public string CodeValues { get; set; }
            public DTO.Enums.TextSearchMethodType SearchMethodType { get; set; }
        }
    }
}