using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class Visits : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.VisitsID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "visits/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "visits/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return null; }
            //get { return "visits/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return null; }
            //get { return "visits/stratifierview.cshtml"; }
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
            get { return "Visits"; }

        }

        public string Description
        {
            get { return "Filter and validate the number of visits the patient has had"; }
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
            get { return new VisitsValues(); }
        }
    }

    public class VisitsValues
    {
        public int? Visits { get; set; }
    }
}