using Lpp.QueryComposer;
using System;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class Prescribing : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.PrescribingID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "Prescribing/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "Prescribing/view.cshtml"; }
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
            get { return "Prescribing"; }

        }

        public string Description
        {
            get { return ""; }
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
            get { return new PrescribingValues(); }
        }
    }

    public class PrescribingValues
    {
        public PrescribingValues()
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