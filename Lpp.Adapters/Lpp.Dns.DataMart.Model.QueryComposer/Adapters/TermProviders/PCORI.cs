using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.TermProviders
{
    [Serializable]
    public class PCORI : IModelTermProvider
    {
        static readonly Guid[] SupportTermIDs = new[] { 
            ModelTermsFactory.TrialID,
            ModelTermsFactory.PatientReportedOutcomeID,
            ModelTermsFactory.PatientReportedOutcomeEncounterID,
            ModelTermsFactory.AgeRangeID,
            ModelTermsFactory.SettingID,
            ModelTermsFactory.ObservationPeriodID,
            ModelTermsFactory.HeightID,
            ModelTermsFactory.WeightID,
            ModelTermsFactory.SexID,
            ModelTermsFactory.RaceID,
            ModelTermsFactory.HispanicID,
            ModelTermsFactory.VitalsMeasureDateID,
            ModelTermsFactory.CombinedDiagnosisCodesID,
            ModelTermsFactory.SqlDistributionID,
            ModelTermsFactory.FileUploadID,
            ModelTermsFactory.ModularProgramID,
            ModelTermsFactory.ProcedureCodesID
        };

        public Guid ModelID
        {
            get { return new Guid("85EE982E-F017-4BC4-9ACD-EE6EE55D2446"); }
        }

        public Guid ProcessorID
        {
            get { return new Guid("AE0DA7B0-0F73-4D06-B70B-922032B7F0EB"); }
        }

        public string Processor
        {
            get { return "Query Composer Model Processor"; }
        }

        public IEnumerable<IModelTerm> Terms
        {
            get { return ModelTermsFactory.Terms.Where(t => SupportTermIDs.Contains(t.ID)).ToArray(); }
        }
    }
}
