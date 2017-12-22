using Lpp.Dns.DTO.Enums;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class Sex : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.SexID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "sex/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "sex/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "sex/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "sex/stratifierview.cshtml"; }
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
            get { return "Sex"; }

        }

        public string Description
        {
            get { return "General lookup of Sex of the visit."; }
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
            get { return new SexValues(); }
        }
    }

    public class SexValues
    {
        public SexStratifications Sex { get; set; }
    }
}