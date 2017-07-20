using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms.DataChecker
{
    public class DiagnosisPDX : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.DC_DiagnosisPDX; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "datachecker/diagnosispdx/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "datachecker/diagnosispdx/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return null; }
        }

        public string StratifierViewRelativePath
        {
            get { return null; }
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
            get { return "Diagnosis PDX"; }
        }

        public string Description
        {
            get { return "Diagnosis PDX"; }
        }

        public string Category
        {
            get { return "Data Checker"; }
        }

        public object ValueTemplate
        {
            get { return new DiagnosisPDXValues(); }
        }

        public class DiagnosisPDXValues
        {

            public DiagnosisPDXValues()
            {
                PDXes = new List<string>();
            }

            public IEnumerable<string> PDXes { get; set; }
        }
    }
}