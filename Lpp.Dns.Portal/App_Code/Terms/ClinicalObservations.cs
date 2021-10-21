using Lpp.QueryComposer;
using System;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class ClinicalObservations : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.ClinicalObservationsID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "ClinicalObservations/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "ClinicalObservations/view.cshtml"; }
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
            get { return "Clinical Observations"; }

        }

        public string Description
        {
            get { return ""; }
        }

        public string Category
        {
            get
            {
                return "Criteria";
            }
        }


        public object ValueTemplate
        {
            get { return new ClinicalObservationsValues(); }
        }
    }

    public class ClinicalObservationsValues
    {
        public ClinicalObservationsValues()
        {
            CodeSet = DTO.Enums.ClinicalObservationsCodeSet.LC;
            CodeValues = string.Empty;
            SearchMethodType = DTO.Enums.TextSearchMethodType.ExactMatch;
        }
        /// <summary>
        /// Gets or sets the code set for the term.
        /// </summary>
        public DTO.Enums.ClinicalObservationsCodeSet CodeSet { get; set; }
        /// <summary>
        /// The values to search on.
        /// </summary>
        public string CodeValues { get; set; }
        /// <summary>
        /// The match method for the terms: 0 = exact match, 1 = starts with
        /// </summary>
        public DTO.Enums.TextSearchMethodType SearchMethodType { get; set; }
        public double? ResultRangeMin { get; set; }
        public double? ResultRangeMax { get; set; }
        public string QualitativeResult { get; set; }
        public DTO.Enums.LOINCResultModifierType? ResultModifier { get; set; }
        public string ResultUnit { get; set; }
    }
}