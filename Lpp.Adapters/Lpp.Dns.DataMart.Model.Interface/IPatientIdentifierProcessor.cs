using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model
{
    /// <summary>
    /// Interface to provide support for executing generation of PATID lists, and reading existing PATID lists for use in future query.
    /// </summary>
    public interface IPatientIdentifierProcessor
    {
        /// <summary>
        /// Gets if the current model/adapter can generate patient identifier lists for the current request.
        /// </summary>
        bool CanGenerateLists { get; }

        /// <summary>
        /// Returns a collection of unique identifiers for each query to generate patient identifier lists for.
        /// </summary>
        /// <returns>For each pair the Key is the query identifier, and the value is a descriptor for the query.</returns>
        IDictionary<Guid, string> GetQueryIdentifiers();
        
        /// <summary>
        /// Executes a query for the current model/adapter that generates a list of PATID's and saves to the local filesystem in the path specified.
        /// </summary>
        /// <param name="requestID">The ID of the request.</param>
        /// <param name="network">The network information for the request.</param>
        /// <param name="md">The request metadata.</param>
        /// <param name="outputPaths">The collection of paths to save the generated PATID lists to. The key value will be the query identifier for the query the output path is for.</param>
        /// <param name="format">The output format of the list, only valid values are 'csv' or an empty or null string.</param>
        void GenerateLists(Guid requestID, NetworkConnectionMetadata network, RequestMetadata md, IDictionary<Guid, string> outputPaths, string format);

        /// <summary>
        /// Provides the paths to the PATID lists to use when executing the query. The index of the path corresponds to the index of the query being executed.
        /// The adapter should cache these values, it will be called prior to the start of the query execution.
        /// </summary>
        /// <param name="inputPaths">The collection of filepaths to the PATID lists. The key value will be the query identifier for the query the input path is for.</param>
        void SetPatientIdentifierSources(IDictionary<Guid, string> inputPaths);
    }
}
