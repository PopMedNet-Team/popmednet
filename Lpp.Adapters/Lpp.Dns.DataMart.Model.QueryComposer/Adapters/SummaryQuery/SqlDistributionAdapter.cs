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

        public SqlDistributionAdapter() : base(QueryComposerModelMetadata.SummaryTableModelID)
        {
        }

        public override void Dispose()
        {
        }

        public override QueryComposerResponseDTO Execute(QueryComposerRequestDTO request, bool viewSQL)
        {
            SummarySqlQueryAdapter sql = new SummarySqlQueryAdapter();
            var result = sql.Execute(request, _settings, viewSQL);
            return result;
        }
    }
}
