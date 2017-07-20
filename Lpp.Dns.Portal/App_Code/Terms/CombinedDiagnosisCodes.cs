using Lpp.Dns.DTO;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class CombinedDiagnosisCodes : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.CombinedDiagnosisCodesID; }
        }

        public string Name
        {
            get { return ModelTermResources.CombinedDiagnosisCodes_Name; }
        }

        public string Description
        {
            get { return ModelTermResources.CombinedDiagnosisCodes_Description; }
        }

        public string Category
        {
            get { return null; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "CombinedDiagnosisCodes/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "CombinedDiagnosisCodes/view.cshtml"; }
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
            get { return new CombinedDiagnosisCodeValues(); }
        }

        public class CombinedDiagnosisCodeValues
        {

            public CombinedDiagnosisCodeValues()
            {
                CodeType = DTO.Enums.DiagnosisCodeTypes.Unknown;
                CodeValues = string.Empty;
                SearchMethodType = DTO.Enums.TextSearchMethodType.ExactMatch;
            }
            /// <summary>
            /// The code set for the term.
            /// </summary>
            public Lpp.Dns.DTO.Enums.DiagnosisCodeTypes CodeType { get; set; }
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