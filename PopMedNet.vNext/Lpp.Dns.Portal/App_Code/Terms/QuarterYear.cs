using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class QuarterYear : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.QuarterYearID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "QuarterYear/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "QuarterYear/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return null; }
            //get { return "QuarterYear/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return null; }
            //get { return "QuarterYear/stratifierview.cshtml"; }
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
            get { return "Year/Quarter Observation Period"; }

        }

        public string Description
        {
            get { return "Filter and validate based on Year/Quarter Observation Period"; }
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
            get { return new QuarterYearValues(); }
        }
    }

    public class QuarterYearValues
    {
        public QuarterYearValues()
        {
            ByYearsOrQuarters = "ByQuarters";
            StartQuarter = "Q1";
            EndQuarter = "Q1";
        }

        public int? StartYear { get; set; }
        public int? EndYear { get; set; }

        public string ByYearsOrQuarters { get; set; }

        public string StartQuarter { get; set; }

        public string EndQuarter { get; set; }
    }
}