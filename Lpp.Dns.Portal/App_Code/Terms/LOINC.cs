using Lpp.QueryComposer;
using System;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class LOINC : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.LOINCCodesID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "LOINC/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "LOINC/view.cshtml"; }
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
            get { return "Lab: LOINC"; }

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
            get { return new LOINCValues(); }
        }
    }

    public class LOINCValues
    {
        public LOINCValues()
        {
            CodeValues = string.Empty;
            SearchMethodType = DTO.Enums.TextSearchMethodType.ExactMatch;
        }
        /// <summary>
        /// The values to search on.
        /// </summary>
        public string CodeValues { get; set; }
        /// <summary>
        /// The match method for the terms: 0 = exact match, 1 = starts with
        /// </summary>
        public DTO.Enums.TextSearchMethodType SearchMethodType { get; set; }
        public int? ResultRangeMin { get; set; }
        public int? ResultRangeMax { get; set; }
        public DTO.Enums.LOINCQualitativeResultType? QualitativeResult { get; set; }
        public DTO.Enums.LOINCResultModifierType? ResultModifier { get; set; }
    }
}