using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class Trial : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.TrialID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "Trial/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "Trial/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "Trial/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "Trial/stratifierview.cshtml"; }
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
            get { return "Trial"; }

        }

        public string Description
        {
            get { return "The ID of the trial the patient participated in."; }
        }

        public string Category
        {
            get
            {
                return "Trial";
            }
        }

        public object ValueTemplate
        {
            get { return new TrialValues(); }
        }
    }

    public class TrialValues
    {
        public string Trial { get; set; }
    }
}