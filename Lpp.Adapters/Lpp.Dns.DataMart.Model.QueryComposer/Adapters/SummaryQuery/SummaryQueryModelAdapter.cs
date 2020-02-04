using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public override void PostProcess(DTO.QueryComposer.QueryComposerResponseDTO response)
        {
            string[] columnNames = LowThresholdColumns(response);

            if (columnNames == null || columnNames.Length == 0)
                return;

            if (!response.Properties.Any(p => p.Name == LowThresholdColumnName))
            {
                //add a LowThreshold property to the definition
                response.Properties = response.Properties.Concat(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = LowThresholdColumnName, As = LowThresholdColumnName, Type = "System.Boolean" } });
            }

            foreach (IEnumerable<Dictionary<string, object>> result in response.Results)
            {
                var table = result;
                foreach (Dictionary<string, object> row in table)
                {
                    try
                    {
                        if (!row.ContainsKey(LowThresholdColumnName))
                        {
                            row.Add(LowThresholdColumnName, false);
                        }

                        bool zeroRow = false;

                        foreach (string column in columnNames)
                        {
                            object currentValue;
                            if (row.TryGetValue(column, out currentValue))
                            {
                                double value;
                                if (currentValue != null && double.TryParse(currentValue.ToString(), out value))
                                {
                                    if (value > 0 && value < _lowThresholdValue)
                                    {
                                        zeroRow = true;
                                        break;
                                    }
                                }

                            }
                        }

                        if (zeroRow)
                        {
                            foreach (string column in columnNames)
                            {
                                row[column] = 0;
                                row[LowThresholdColumnName] = true;
                            }
                        }
                    }
                    catch { }
                }
            }

            _currentResponse = response;
        }
    }
}
