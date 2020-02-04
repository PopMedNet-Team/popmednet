using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.DTO.QueryComposer;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public abstract class SummaryQueryModelAdapter : ModelAdapter
    {
        public SummaryQueryModelAdapter(Guid modelID, RequestMetadata requestMetadata) : base(modelID, requestMetadata) { }

        protected override string[] LowThresholdColumns(DTO.QueryComposer.QueryComposerResponseDTO response)
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

        protected DTO.QueryComposer.QueryComposerResponseDTO _currentResponse = null;

        public override QueryComposerModelProcessor.DocumentEx[] OutputDocuments()
        {
            if (_currentResponse == null)
                return new QueryComposerModelProcessor.DocumentEx[0];

            return new[] { SerializeResponse(_currentResponse, QueryComposerModelProcessor.NewGuid(), "response.json") };
        }

        public override void PostProcess(QueryComposerResponseDTO response)
        {
            base.PostProcess(response);

            _currentResponse = response;
        }
    }
}
