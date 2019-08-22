using log4net;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public class MostFrequentlyUsedQueriesModelAdapter : SummaryQueryModelAdapter
    {
        static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static readonly Guid[] CodeQueryTermIDs;

        static MostFrequentlyUsedQueriesModelAdapter()
        {
            CodeQueryTermIDs = new[] { ModelTermsFactory.HCPCSProcedureCodesID, 
                                           ModelTermsFactory.ICD9DiagnosisCodes3digitID,
                                           ModelTermsFactory.ICD9DiagnosisCodes4digitID,
                                           ModelTermsFactory.ICD9DiagnosisCodes5digitID,
                                           ModelTermsFactory.ICD9ProcedureCodes3digitID,
                                           ModelTermsFactory.ICD9ProcedureCodes4digitID };
        }

        public MostFrequentlyUsedQueriesModelAdapter(RequestMetadata requestMetadata) : base(QueryComposerModelMetadata.SummaryTableModelID, requestMetadata) { }

        public override DTO.QueryComposer.QueryComposerResponseDTO Execute(DTO.QueryComposer.QueryComposerRequestDTO request, bool viewSQL)
        {
            Guid termID = Guid.Empty;
            IQueryAdapter queryAdapter = null;
            if (request.Where.Criteria.First().Terms.Any(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.SqlDistributionID))
            {
                logger.Debug("Sql Distribution term found, creating SummarySqlQueryAdapter.");
                SummarySqlQueryAdapter sql = new SummarySqlQueryAdapter();
                var result = sql.Execute(request, _settings, viewSQL);

                _currentResponse = result;

                return result;
            }

            if (request.Select.Fields.Any(f => CodeQueryTermIDs.Contains(f.Type)))
            {
                if (request.Select.Fields.Any(f => f.Type == ModelTermsFactory.HCPCSProcedureCodesID))
                {
                    termID = ModelTermsFactory.HCPCSProcedureCodesID;
                    logger.Debug("HCPCS Procedure Codes selector found, creating MFUCodesQueryAdapter.");
                }
                else if (request.Select.Fields.Any(f => f.Type == ModelTermsFactory.ICD9DiagnosisCodes3digitID))
                {
                    termID = ModelTermsFactory.ICD9DiagnosisCodes3digitID;
                    logger.Debug("ICD9 Diagnosis Codes (3 digit) selector found, creating MFUCodesQueryAdapter.");
                }
                else if (request.Select.Fields.Any(f => f.Type == ModelTermsFactory.ICD9DiagnosisCodes4digitID))
                {
                    termID = ModelTermsFactory.ICD9DiagnosisCodes4digitID;
                    logger.Debug("ICD9 Diagnosis Codes (4 digit) term found, creating MFUCodesQueryAdapter.");
                }
                else if (request.Select.Fields.Any(f => f.Type == ModelTermsFactory.ICD9DiagnosisCodes5digitID))
                {
                    termID = ModelTermsFactory.ICD9DiagnosisCodes5digitID;
                    logger.Debug("ICD9 Diagnosis Codes (5 digit) selector found, creating MFUCodesQueryAdapter.");
                }
                else if (request.Select.Fields.Any(f => f.Type == ModelTermsFactory.ICD9ProcedureCodes3digitID))
                {
                    termID = ModelTermsFactory.ICD9ProcedureCodes3digitID;
                    logger.Debug("ICD9 Procedure Codes (3 digit) selector found, creating MFUCodesQueryAdapter.");
                }
                else if (request.Select.Fields.Any(f => f.Type == ModelTermsFactory.ICD9ProcedureCodes4digitID))
                {
                    termID = ModelTermsFactory.ICD9ProcedureCodes4digitID;
                    logger.Debug("ICD9 Procedure Codes (4 digit) selector found, creating MFUCodesQueryAdapter.");
                }

                queryAdapter = new MostFrequentlyUsedCodesQueryAdapter(_settings, termID);
            }
            else if (request.Select.Fields.Any(f => f.Type == ModelTermsFactory.DrugClassID || f.Type == ModelTermsFactory.DrugNameID))
            {
                if (request.Select.Fields.Any(f => f.Type == ModelTermsFactory.DrugClassID))
                {
                    termID = ModelTermsFactory.DrugClassID;
                    logger.Debug("Pharamcy dispensing drug class selector found, creating MFUPharmaQueryAdapter.");
                }
                else if (request.Select.Fields.Any(f => f.Type == ModelTermsFactory.DrugNameID))
                {
                    termID = ModelTermsFactory.DrugNameID;
                    logger.Debug("Pharamcy dispensing generic drug selector found, creating MFUPharmaQueryAdapter.");
                }

                queryAdapter = new MostFrequentlyUsedPharmaDispensingQueryAdapter(_settings, termID);
            }

            if (queryAdapter == null)
            {
                throw new InvalidOperationException("Unable to determine the query adapter to use based on the primary criteria terms.");
            }

            using (queryAdapter)
            {
                var qcResponseDTO = queryAdapter.Execute(request, viewSQL);
                qcResponseDTO.LowCellThrehold = _lowThresholdValue;

                _currentResponse = qcResponseDTO;

                return qcResponseDTO;
            }
        }

        public override void Dispose()
        {
        }
    }
}
