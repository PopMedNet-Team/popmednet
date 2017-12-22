using Lpp.Dns.DTO.Enums;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class Criteria : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.CriteriaID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "Criteria/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "Criteria/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return null; }
            //get { return "Criteria/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return null; }
            //get { return "Criteria/stratifierview.cshtml"; }
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
            get { return "Output Criteria"; }

        }

        public string Description
        {
            get { return "General lookup of Output Criteria of the visit."; }
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
            get { return new CriteriaValues(); }
        }
    }

    public class CriteriaValues
    {
        public OutputCriteria Criteria { get; set; }
    }
}