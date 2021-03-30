using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO.QueryComposer;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public class SqlDistributionAdapter : SummaryQueryModelAdapter
    {

        public SqlDistributionAdapter(RequestMetadata requestMetadata) : base(QueryComposerModelMetadata.SummaryTableModelID, requestMetadata)
        {
        }

        public override void Dispose()
        {
        }

        public override IEnumerable<QueryComposerResponseQueryResultDTO> Execute(QueryComposerQueryDTO query, bool viewSQL)
        {
            SummarySqlQueryAdapter sql = new SummarySqlQueryAdapter();
            var result = sql.Execute(query, _settings, viewSQL);
            return new[] { result };
        }
    }
}
