using Lpp.Dns.DTO;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class ProcedureCodes : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.ProcedureCodesID; }
        }

        public string Name
        {
            get { return ModelTermResources.ProcedureCode_Name; }
        }

        public string Description
        {
            get { return ModelTermResources.ProcedureCode_Description; }
        }

        public string Category
        {
            get { return null; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "ProcedureCodes/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "ProcedureCodes/view.cshtml"; }
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

        public object ValueTemplate
        {
            get { return new ProcedureCodesValues(); }
        }

        public class ProcedureCodesValues
        {

            public ProcedureCodesValues()
            {
                CodeType = DTO.Enums.ProcedureCode.Any;
                CodeValues = string.Empty;
                SearchMethodType = DTO.Enums.TextSearchMethodType.ExactMatch;
            }
            /// <summary>
            /// The code set for the term.
            /// </summary>
            public Lpp.Dns.DTO.Enums.ProcedureCode CodeType { get; set; }
            /// <summary>
            /// The values to search on.
            /// </summary>
            public string CodeValues { get; set; }
            /// <summary>
            /// The match method for the terms: 0 = exact match, 1 = starts with
            /// </summary>
            public DTO.Enums.TextSearchMethodType SearchMethodType { get; set; }
        } 
    }
    
}