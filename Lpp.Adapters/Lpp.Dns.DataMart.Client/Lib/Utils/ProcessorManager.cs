using System;
using System.Collections.Generic;
using Lpp.Dns.DataMart.Model;

namespace Lpp.Dns.DataMart.Lib.Utils
{
    public class ProcessorManager
    {
        public static void UpdateProcessorSettings(ModelDescription modelDescription, IModelProcessor processor)
        {
            Dictionary<string, object> settings = new Dictionary<string, object>();

            if (modelDescription.Properties != null && modelDescription.Properties.Count > 0)
            {
                foreach (PropertyData property in modelDescription.Properties)
                {
                    settings.Add(property.Name, property.Value);
                }

                if (!settings.ContainsKey("AppFolderPath"))
                {
                    settings.Add("AppFolderPath", Lpp.Dns.DataMart.Client.Utils.Configuration.AppFolderPath);
                }
            }

            processor.Settings = settings;
        }

    }
}
