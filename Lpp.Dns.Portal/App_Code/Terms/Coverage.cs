using Lpp.Dns.DTO.Enums;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class Coverage : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.CoverageID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "Coverage/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "Coverage/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "Coverage/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "Coverage/stratifierview.cshtml"; }
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
            get { return "Coverages"; }

        }

        public string Description
        {
            get { return "General lookup of Coverages of the visit."; }
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
            get { return new CoverageValues(); }
        }
    }

    public class CoverageValues
    {
        public Coverages Coverage { get; set; }
    }
}