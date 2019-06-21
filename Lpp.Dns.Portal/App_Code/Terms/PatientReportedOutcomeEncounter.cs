using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class PatientReportedOutcomeEncounter : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.PatientReportedOutcomeEncounterID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return null; }
        }

        public string CriteriaViewRelativePath
        {
            get { return null; }
        }

        public string StratifierEditRelativePath
        {
            get { return "PatientReportedOutcomeEncounter/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "PatientReportedOutcomeEncounter/stratifierview.cshtml"; }
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
            get { return "Patient Reported Outcome Encounters"; }

        }

        public string Description
        {
            get { return "A term to allow the inclusion of Encounters based on PRO measures."; }
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
            get { return null; }
        }
    }

    //public class PatientReportedOutcomeEncounterValues
    //{
    //    public string ItemName { get; set; }
    //    public string ItemResponse { get; set; }
    //}
}