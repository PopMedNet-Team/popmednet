using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.TermProviders
{
    [Serializable]
    public class SummaryTable : IModelTermProvider
    {
        static readonly Guid[] SupportTermIDs = new[] {
            ModelTermsFactory.SettingID,
            ModelTermsFactory.AgeRangeID,
            ModelTermsFactory.SexID,
            ModelTermsFactory.YearID,
            ModelTermsFactory.QuarterYearID,
            ModelTermsFactory.ICD9DiagnosisCodes3digitID,
            ModelTermsFactory.ICD9DiagnosisCodes4digitID,
            ModelTermsFactory.ICD9DiagnosisCodes5digitID,
            ModelTermsFactory.ICD9ProcedureCodes3digitID,
            ModelTermsFactory.ICD9ProcedureCodes4digitID,
            ModelTermsFactory.HCPCSProcedureCodesID,
            ModelTermsFactory.DrugClassID,
            ModelTermsFactory.DrugNameID,
            ModelTermsFactory.CoverageID,
            ModelTermsFactory.CodeMetricID,
            ModelTermsFactory.DispensingMetricID,
            ModelTermsFactory.CriteriaID,
            ModelTermsFactory.SqlDistributionID,
            ModelTermsFactory.FileUploadID,
            ModelTermsFactory.ModularProgramID
        };

        public Guid ModelID
        {
            get { return new Guid("CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB"); }
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
