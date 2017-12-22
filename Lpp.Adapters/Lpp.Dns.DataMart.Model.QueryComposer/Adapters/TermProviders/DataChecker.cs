using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.TermProviders
{
    [Serializable]
    public class DataChecker : IModelTermProvider
    {
        static readonly Guid[] SupportTermIDs = new[] { 
            ModelTermsFactory.DC_DataPartners,
            ModelTermsFactory.DC_DiagnosisPDX,
            ModelTermsFactory.DC_DispensingRXAmount,
            ModelTermsFactory.DC_DispensingRXSupply,
            ModelTermsFactory.DC_Encounter,
            ModelTermsFactory.DC_Ethnicity,
            ModelTermsFactory.DC_DiagnosisCodes,
            ModelTermsFactory.DC_ProcedureCodes,
            ModelTermsFactory.DC_MetadataCompleteness,
            ModelTermsFactory.DC_NDCCodes,
            ModelTermsFactory.DC_Race,
            ModelTermsFactory.DC_AgeDistribution,
            ModelTermsFactory.DC_HeightDistribution,
            ModelTermsFactory.DC_WeightDistribution,
            ModelTermsFactory.DC_SexDistribution,
            ModelTermsFactory.SqlDistributionID,
            ModelTermsFactory.FileUploadID,
            ModelTermsFactory.ModularProgramID
        };

        public Guid ModelID
        {
            get { return new Guid("321ADAA1-A350-4DD0-93DE-5DE658A507DF"); }
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
