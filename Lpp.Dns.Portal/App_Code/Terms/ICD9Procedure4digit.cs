using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class ICD9Procedure4digit : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.ICD9ProcedureCodes4digitID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "icd9procedure4digit/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "icd9procedure4digit/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "ICD9Procedure4digit/EditStratifierView.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "ICD9Procedure4digit/StratifierView.cshtml"; }
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
            get { return "ICD-9 Procedures (4 digit)"; }

        }

        public string Description
        {
            get { return "Filter and validate based on 4 digit ICD-9 Procedures Codes"; }
        }

        public string Category
        {
            get
            {
                return "ICD 9";
            }
        }


        public object ValueTemplate
        {
            get { return new ICD9ProcedureValues(); }
        }
    }
}