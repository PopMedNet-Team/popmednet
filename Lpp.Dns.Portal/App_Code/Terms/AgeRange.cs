using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class AgeRange : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.AgeRangeID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "agerange/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "agerange/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "agerange/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "agerange/stratifierview.cshtml"; }
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
            get { return "Age Range"; }

        }

        public string Description
        {
            get { return "Filter and validate based on age range of the patient"; }
        }

        public string Category
        {
            get
            {
                return "Demographic";
            }
        }


        public object ValueTemplate
        {
            get { return new AgeRangeValues(); }
        }
    }

    public class AgeRangeValues
    {

        public AgeRangeValues()
        {
            CalculateAsOfRequired = false;
        }

        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public DTO.Enums.AgeRangeCalculationType CalculationType { get; set; }
        public DateTime? CalculateAsOf { get; set; }

        //This is a helper property to assist in UI, not needed for the actual request
        public bool? CalculateAsOfRequired { get; set; }
    }
}