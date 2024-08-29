using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model
{
    /// <summary>
    /// An implementation of the IModelProcessor allows the exchange and processing of Documents between the DataMartClient
    /// and the DataMart for a specific Model.
    /// </summary>
    public interface IModelMetadata
    {
        /// <summary>
        /// Returns the Model Name.
        /// </summary>
        string ModelName { get; }

        /// <summary>
        /// Returns the Model Id.
        /// </summary>
        Guid ModelId { get; }

        /// <summary>
        /// Returns the Model Version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// List of capabilities of the Model.
        /// </summary>
        IDictionary<string, bool> Capabilities { get; }

        /// <summary>
        /// List of properties of the Model. These are properties whose values will be stored by the DataMartClient.
        /// </summary>
        IDictionary<string, string> Properties { get; }

        /// <summary>
        /// List of the properties the processor needs.
        /// </summary>
        ICollection<Settings.ProcessorSetting> Settings { get; }

        /// <summary>
        /// Gets the type of sql providers the processor supports.
        /// </summary>
        IEnumerable<Settings.SQLProvider> SQlProviders { get; }

    }
}
