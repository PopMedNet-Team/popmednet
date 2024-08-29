using log4net;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public class IncidenceModelAdapter : SummaryQueryModelAdapter
    {
        static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IncidenceModelAdapter(RequestMetadata requestMetadata) : base(QueryComposerModelMetadata.SummaryTableModelID, requestMetadata) { }

        public override IEnumerable<QueryComposerResponseQueryResultDTO> Execute(QueryComposerQueryDTO query, bool viewSQL)
        {
            IQueryAdapter queryAdapter = null;
            if (query.Where.Criteria.First().Terms.Any(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.SqlDistributionID))
            {
                logger.Debug("Sql Distribution term found, creating SummarySqlQueryAdapter.");
                SummarySqlQueryAdapter sql = new SummarySqlQueryAdapter();
                var result = sql.Execute(query, _settings, viewSQL);
                return new[] { result };
            }

            if (query.Select.Fields.Any(t => t.Type == ModelTermsFactory.ICD9DiagnosisCodes3digitID))
            {
                logger.Debug("ICD-9 Diagnoses Codes term found, creating IncidenceICD9DiagnosisQueryAdapter.");

                queryAdapter = new IncidenceICD9DiagnosisQueryAdapter(_settings);
            }
            else if (query.Select.Fields.Any(t => t.Type == ModelTermsFactory.DrugNameID) || query.Select.Fields.Any(t => t.Type == ModelTermsFactory.DrugClassID))
            {
                if (query.Select.Fields.Any(t => t.Type == ModelTermsFactory.DrugNameID))
                    logger.Debug("Pharmacy dispensing generic drug name term found, creating IncidencePharmaDispensingQueryAdapter.");
                else
                    logger.Debug("Pharmacy dispensing drug class term found, creating IncidencePharmaDispensingQueryAdapter.");

                queryAdapter = new IncidencePharmaDispensingQueryAdapter(_settings);
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
