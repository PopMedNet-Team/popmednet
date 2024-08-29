using log4net;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public class PrevalenceModelAdapter : SummaryQueryModelAdapter
    {
        static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static readonly Guid[] CodeQueryTermIDs;

        static PrevalenceModelAdapter()
        {
            CodeQueryTermIDs = new[] { ModelTermsFactory.ICD9DiagnosisCodes3digitID,
                                       ModelTermsFactory.ICD9DiagnosisCodes4digitID,
                                       ModelTermsFactory.ICD9DiagnosisCodes5digitID,
                                       ModelTermsFactory.ICD9ProcedureCodes3digitID,
                                       ModelTermsFactory.ICD9ProcedureCodes4digitID };
        }

        public PrevalenceModelAdapter(RequestMetadata requestMetadata) : base(QueryComposerModelMetadata.SummaryTableModelID, requestMetadata) { }

        public override IEnumerable<DTO.QueryComposer.QueryComposerResponseQueryResultDTO> Execute(DTO.QueryComposer.QueryComposerQueryDTO query, bool viewSQL)
        {
            IQueryAdapter queryAdapter = null;
            if (query.Where.Criteria.First().Terms.Any(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.SqlDistributionID))
            {
                logger.Debug("Sql Distribution term found, creating SummarySqlQueryAdapter.");
                SummarySqlQueryAdapter sql = new SummarySqlQueryAdapter();
                var result = sql.Execute(query, _settings, viewSQL);
                return new[] { result };
            }

            if (query.Select.Fields.Any(t => t.Type == ModelTermsFactory.DrugNameID) || query.Select.Fields.Any(t => t.Type == ModelTermsFactory.DrugClassID))
            {
                if (query.Select.Fields.Any(t => t.Type == ModelTermsFactory.DrugNameID))
                    logger.Debug("Pharmacy dispensing generic drug name term found, creating PrevalencePharmaDispensingQueryAdapter.");
                else
                    logger.Debug("Pharmacy dispensing drug class term found, creating PrevalencePharmaDispensingQueryAdapter.");

                queryAdapter = new PrevalencePharmaDispensingQueryAdapter(_settings);
            }
            else if (query.Select.Fields.Any(t => t.Type == ModelTermsFactory.CoverageID))
            {
                logger.Debug("Coverage term found, creating PrevalenceEnrollmentQueryAdapter.");

                queryAdapter = new PrevalenceEnrollmentQueryAdapter(_settings);
            }
            else if (query.Select.Fields.Any(t => t.Type == ModelTermsFactory.HCPCSProcedureCodesID))
            {
                logger.Debug("HCPCS procedure codes term found, creating PrevalenceHCPCSProceduresQueryAdapter.");

                queryAdapter = new PrevalenceHCPCSProceduresQueryAdapter(_settings);
            }
            else if (query.Select.Fields.Any(f => CodeQueryTermIDs.Contains(f.Type)))
            {
                Guid termID = Guid.Empty;
                if (query.Select.Fields.Any(t => t.Type == ModelTermsFactory.ICD9ProcedureCodes3digitID))
                {
                    termID = ModelTermsFactory.ICD9ProcedureCodes3digitID;
                    logger.Debug("ICD9 Procedure Codes (3 digit) term found, creating PrevalenceICD9QueryAdapter.");
                }
                else if (query.Select.Fields.Any(t => t.Type == ModelTermsFactory.ICD9ProcedureCodes4digitID))
                {
                    termID = ModelTermsFactory.ICD9ProcedureCodes4digitID;
                    logger.Debug("ICD9 Procedure Codes (4 digit) term found, creating PrevalenceICD9QueryAdapter.");
                }
                else if (query.Select.Fields.Any(t => t.Type == ModelTermsFactory.ICD9DiagnosisCodes3digitID))
                {
                    termID = ModelTermsFactory.ICD9DiagnosisCodes3digitID;
                    logger.Debug("ICD9 Diagnosis Codes (3 digit) term found, creating PrevalenceICD9QueryAdapter.");
                }
                else if (query.Select.Fields.Any(t => t.Type == ModelTermsFactory.ICD9DiagnosisCodes4digitID))
                {
                    termID = ModelTermsFactory.ICD9DiagnosisCodes4digitID;
                    logger.Debug("ICD9 Diagnosis Codes (4 digit) term found, creating PrevalenceICD9QueryAdapter.");
                }
                else if (query.Select.Fields.Any(t => t.Type == ModelTermsFactory.ICD9DiagnosisCodes5digitID))
                {
                    termID = ModelTermsFactory.ICD9DiagnosisCodes5digitID;
                    logger.Debug("ICD9 Diagnosis Codes (5 digit) term found, creating PrevalenceICD9QueryAdapter.");
                }

                queryAdapter = new PrevalenceICD9QueryAdapter(_settings, termID);
            }

            if (queryAdapter == null)
            {
                throw new InvalidOperationException("Unable to determine the query adapter to use based on the primary criteria terms.");
            }

            using (queryAdapter)
            {
                var qcResponseDTO = queryAdapter.Execute(query, viewSQL);
                foreach(var r in qcResponseDTO)
                {
                    r.LowCellThrehold = _lowThresholdValue;
                }

                return qcResponseDTO;
            }
        }

        public override void Dispose()
        {
        }
    }
}
