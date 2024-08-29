using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class PatientReportedOutcome : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.PatientReportedOutcomeID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "PatientReportedOutcome/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "PatientReportedOutcome/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "PatientReportedOutcome/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "PatientReportedOutcome/stratifierview.cshtml"; }
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
            get { return "Patient Reported Outcome (PRO)"; }

        }

        public string Description
        {
            get { return "A term to allow querying against the PRO_CM table for patient-reported outcome measures."; }
        }

        public string Category
        {
            get
            {
                return "Patient Reported <br/> Outcome";
            }
        }

        public object ValueTemplate
        {
            get { return new PatientReportedOutcomeValues(); }
        }
    }

    public class PatientReportedOutcomeValues
    {
        public string ItemName { get; set; }
        public string ItemResponse { get; set; }
    }
}