using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms.DataChecker
{
    public class DiagnosisCodes : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.DC_DiagnosisCodes; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "datachecker/diagnosiscodes/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "datachecker/diagnosiscodes/view.cshtml"; }
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
            get { return ModelTermResources.DC_DiagnosisCodes_Name; }
        }

        public string Description
        {
            get { return ModelTermResources.DC_DiagnosisCodes_Description; }
        }

        public string Category
        {
            get { return "Data Checker"; }
        }

        public object ValueTemplate
        {
            get { return new DiagnosisCodeValues(); }
        }
        public class DiagnosisCodeValues
        {
            public DiagnosisCodeValues()
            {
                CodeType = string.Empty;
                CodeValues = string.Empty;
                SearchMethodType = DTO.Enums.TextSearchMethodType.ExactMatch;
            }


            public string CodeType { get; set; }
            public string CodeTypeName { get; set; }
            public string CodeValues { get; set; }
            public DTO.Enums.TextSearchMethodType SearchMethodType { get; set; }
        }
    }
}