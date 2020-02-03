using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.TermProviders
{
    [Serializable]
    public class ESP : IModelTermProvider
    {
        //based on the terms used in the original Lpp.Dns.DataMart.Model.ESPQueryBuilder.QueryComposer.xsl used for the queries
        static readonly Guid[] SupportTermIDs = new[] {            
            ModelTermsFactory.AgeRangeID,
            ModelTermsFactory.EthnicityID,
            ModelTermsFactory.ConditionsID,
            ModelTermsFactory.ICD9DiagnosisCodes3digitID,
            ModelTermsFactory.ICD9DiagnosisCodes4digitID,
            ModelTermsFactory.ICD9DiagnosisCodes5digitID,
            ModelTermsFactory.ObservationPeriodID,            
            ModelTermsFactory.RaceID,
            ModelTermsFactory.SexID,
            ModelTermsFactory.TobaccoUseID,
            ModelTermsFactory.VisitsID,
            ModelTermsFactory.ZipCodeID,
            ModelTermsFactory.SqlDistributionID,
            ModelTermsFactory.ESPDiagnosisCodesID,
            ModelTermsFactory.FileUploadID,
            ModelTermsFactory.ModularProgramID
        };

        public ESP()
        {
        }

        public Guid ModelID
        {
            get { return new Guid("7C69584A-5602-4FC0-9F3F-A27F329B1113"); }
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
