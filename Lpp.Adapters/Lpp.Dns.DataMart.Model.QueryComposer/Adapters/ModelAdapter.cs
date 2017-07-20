using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters
{
    public abstract class ModelAdapter : IModelAdapter
    {
        readonly Guid _modelID;
        protected IDictionary<string, object> _settings = null;
        protected double? _lowThresholdValue = null;

        public ModelAdapter(Guid modelID)
        {
            _modelID = modelID;
        }

        public Guid ModelID 
        { 
            get 
            { 
                return _modelID;
            } 
        }

        public virtual bool CanViewSQL
        {
            get { return true; }
        }

        public virtual bool CanRunAndUpload
        {
            get { return true; }
        }

        public virtual bool CanUploadWithoutRun
        {
            get { return false; }
        }

        public virtual bool CanAddResponseFiles
        {
            get { return false; }
        }

        public virtual void Initialize(IDictionary<string, object> settings)
        {
            _settings = settings;

            object settingValue;
            if (settings != null && settings.TryGetValue("LowThresholdValue", out settingValue))
            {
                double value;
                if (double.TryParse((settingValue ?? string.Empty).ToString(), out value))
                    _lowThresholdValue = value;
            }
        }

        public abstract DTO.QueryComposer.QueryComposerResponseDTO Execute(DTO.QueryComposer.QueryComposerRequestDTO request, bool viewSQL);

        public virtual bool CanPostProcess(DTO.QueryComposer.QueryComposerResponseDTO response, out string message)
        {
            message = string.Empty;
            bool hasErrors = response.Errors != null && response.Errors.Any();

            //NOTE: Need to declare hasLowThreshold with a value because at this point in time response doesnt have the LowThreshold Column added to the response yet.
            if (response.Results != null && !hasErrors && _lowThresholdValue.HasValue && response.Results.Any())
            {
                Func<KeyValuePair<string,object>, double?> parseDouble = (KeyValuePair<string, object> v) => {
                    if (v.Value == null)
                        return null;

                    double result;
                    return double.TryParse(v.Value.ToString(), out result) ? result : new Nullable<double>();
                };

                var xx = from result in response.Results
                         from row in result
                         from col in row
                         let val = parseDouble(col)
                         where col.Value != null && col.Value.GetType() != typeof(string)
                         && val.HasValue
                         && val.Value > 0
                         && val.Value < _lowThresholdValue
                         select col;

                if (xx.Any())
                {
                    //at least one cell has a value that falls below the low threshold value.
                    message = "The query results may have rows with low cell counts. You can choose to set the low cell count data to 0 by clicking the [Suppress Low Cell] button.";
                }

            }

            return _lowThresholdValue.HasValue;
        }

        protected virtual string[] LowThresholdColumns(DTO.QueryComposer.QueryComposerResponseDTO response)
        {
            return new string[0];
        }

        public const string LowThresholdColumnName = "LowThreshold";

        public virtual void PostProcess(DTO.QueryComposer.QueryComposerResponseDTO response)
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
                                        row[column] = 0;
                                        row[LowThresholdColumnName] = true;
                                    }
                                }

                            }
                        }
                    }
                    catch { }
                }
            }
        }

        public abstract void Dispose();


        
    }
}
