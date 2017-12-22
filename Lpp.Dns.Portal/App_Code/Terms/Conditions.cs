using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class Conditions : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.ConditionsID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "conditions/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "conditions/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "conditions/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "conditions/stratifierview.cshtml"; }
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
            get { return "Conditions"; }

        }

        public string Description
        {
            get { return "General lookup of conditions of the visit."; }
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
            get { return new ConditionValues(); }
        }
    }

    public class ConditionValues
    {
        public ConditionClassifications Condition { get; set; }
    }
}