using Lpp.Dns.DTO.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer
{
    public interface IModelAdapter : IDisposable
    {
        /// <summary>
        /// Gets the ModelID the adapter supports.
        /// </summary>
        Guid ModelID { get; }

        /// <summary>
        /// Gets if the adapter supports getting the sql.
        /// </summary>
        bool CanViewSQL { get; }

        /// <summary>
        /// Gets if the adapter supports running and uploading.
        /// </summary>
        bool CanRunAndUpload { get; }

        /// <summary>
        /// Gets if the adapter supports uploading without running.
        /// </summary>
        bool CanUploadWithoutRun { get; }

        /// <summary>
        /// Gets if the user can upload response files.
        /// </summary>
        bool CanAddResponseFiles { get; }

        /// <summary>
        /// Gets if the adapter supports generating patient identifier lists.
        /// </summary>
        bool CanGeneratePatientIdentifierLists { get; }

        /// <summary>
        /// Initializes the adapter with the provided settings.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="requestID">The ID of the request being executed.</param>
        void Initialize(IDictionary<string, object> settings, string requestID);

        /// <summary>
        /// Executes the specified request, if viewSQL is true only the response will only contain the sql for the query.
        /// </summary>
        /// <param name="request">The request criteria.</param>
        /// <param name="viewSQL">Indicates if the request should be fully executed and results returned, or just the query sql.</param>
        /// <returns></returns>
        QueryComposerResponseDTO Execute(QueryComposerRequestDTO request, bool viewSQL);

        /// <summary>
        /// Returns the output documents for the last execution of the model adapter.
        /// </summary>
        /// <returns></returns>
        QueryComposerModelProcessor.DocumentEx[] OutputDocuments();

        /// <summary>
        /// Indicates if the adapter can provide post processing for the response.
        /// </summary>
        /// <param name="response">The response that would have post processing performed on it.</param>
        /// <returns></returns>
        bool CanPostProcess(QueryComposerResponseDTO response, out string message);

        /// <summary>
        /// Performs any post-processing required on the specified response.
        /// </summary>
        /// <param name="response"></param>
        void PostProcess(QueryComposerResponseDTO response);

        /// <summary>
        /// Executes the current queries to generate Patient Identifier Lists and save to the specified file paths.
        /// </summary>
        /// <param name="outputPaths"></param>
        void GeneratePatientIdentifierLists(DTO.QueryComposer.QueryComposerRequestDTO request, IDictionary<Guid, string> outputPaths, string format);
    }
}
