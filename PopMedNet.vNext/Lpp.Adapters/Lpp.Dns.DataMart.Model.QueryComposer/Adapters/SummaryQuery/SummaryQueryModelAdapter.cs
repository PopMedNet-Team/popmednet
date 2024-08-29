using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public abstract class SummaryQueryModelAdapter : ModelAdapter
    {
        public SummaryQueryModelAdapter(Guid modelID, RequestMetadata requestMetadata) : base(modelID, requestMetadata) { }

        protected override string[] LowThresholdColumns(DTO.QueryComposer.QueryComposerResponseQueryResultDTO response)
        {
            List<string> ltCols = new List<string>();

            foreach (IEnumerable<Dictionary<string, object>> table in response.Results)
            {
                Dictionary<string, object> row = table.FirstOrDefault();
                if (row != null && row.Count > 0)
                {
                    try
                    {
                        foreach (string column in row.Keys)
                        {
                            if (SummaryQueryUtil.IsCheckedColumn(column))
                            {
                                ltCols.Add(column);
                                var depCols = SummaryQueryUtil.GetDependentComputedColumns(column, row);
                                if (depCols != null && depCols.Count > 0)
                                    ltCols.AddRange(depCols);
                            }
                        }
                    }
                    catch { }
                }
            }

            return ltCols.ToArray();
        }
    }
}
