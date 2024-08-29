using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class Year : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.YearID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "Year/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "Year/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return null; }
            //get { return "Year/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return null; }
            //get { return "Year/stratifierview.cshtml"; }
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
            get { return "Year Observation Period"; }

        }

        public string Description
        {
            get { return "Filter and validate based on Year Observation Period"; }
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
            get { return new YearValues(); }
        }
    }

    public class YearValues
    {
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
    }
}