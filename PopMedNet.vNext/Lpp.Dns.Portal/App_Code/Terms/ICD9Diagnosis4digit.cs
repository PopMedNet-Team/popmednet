﻿using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class ICD9Diagnosis4digit : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.ICD9DiagnosisCodes4digitID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "icd9diagnosis4digit/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "icd9diagnosis4digit/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "icd9diagnosis4digit/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "icd9diagnosis4digit/stratifierview.cshtml"; }
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
            get { return "ICD-9 Diagnosis (4 digit)"; }

        }

        public string Description
        {
            get { return "Filter and validate based on 4 digit ICD-9 Diagnosis Codes"; }
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
            get { return new ICD9DiagnosisValues(); }
        }
    }
}