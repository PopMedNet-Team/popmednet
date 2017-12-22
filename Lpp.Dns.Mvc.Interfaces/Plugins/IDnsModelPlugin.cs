using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reactive;
using System.IO;
using System.Web.Mvc;
using System.Collections;
using Lpp.Dns.DTO;
using Lpp.Dns.Data;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns
{
    public interface IDnsIFrameModelPlugin : IDnsModelPlugin
    {
        /// <summary>
        /// Must have {ID} as a parameter of the url to be replaced, and it must pass the SID.
        /// </summary>
        string EditUrl { get; }
        string ViewUrl { get; }
    }

    public interface IDnsModelPlugin
    {
        /// <summary>
        /// Gets the plugin version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// List of models that this plugin supports (or "provides")
        /// </summary>
        IEnumerable<IDnsModel> Models { get; }

        /// <summary>
        /// Returns a view that visualizes the given request
        /// </summary>
        Func<HtmlHelper, IHtmlString> DisplayRequest( IDnsRequestContext context );

        /// <summary>
        /// Returns a view that visualizes the configuration form for the model processor
        /// </summary>
        Func<HtmlHelper, IHtmlString> DisplayConfigurationForm( IDnsModel model, Dictionary<string, string> propertiesDict );

        /// <summary>
        /// Returns a view that visualizes the given response
        /// </summary>
        Func<HtmlHelper, IHtmlString> DisplayResponse( IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode );


        /// <summary>
        /// Returns a list of export formats that are supported for the given response.
        /// Null return is treated as empty sequence.
        /// </summary>
        IEnumerable<IDnsResponseExportFormat> GetExportFormats( IDnsResponseContext context );

        /// <summary>
        /// Exports the given response in the given format
        /// </summary>
        /// <param name="args">Optional arguments</param>
        IDnsDocument ExportResponse( IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format, string args );


        /// <summary>
        /// Returns a view representing input fields (or some more complex UI) for editing or creating a request
        /// </summary>
        Func<HtmlHelper, IHtmlString> EditRequestView( IDnsRequestContext context );

        /// <summary>
        /// Returns the same view as <see cref="EditRequestView"/> in response to a form post,
        /// which is typically caused by an error while processing the post.
        /// </summary>
        Func<HtmlHelper, IHtmlString> EditRequestReDisplay( IDnsRequestContext request, IDnsPostContext post );

        /// <summary>
        /// Processes an HTTP POST that came from the view produced by the <see cref="EditRequestView"/> method
        /// <param name="context">Request context identifying the current request.</param>
        /// <param name="post">The form post.</param>
        /// </summary>
        DnsRequestTransaction EditRequestPost( IDnsRequestContext request, IDnsPostContext post );

        /// <summary>
        /// Gives the plugin an opportunity to shift the time-based parameters of the request given the time difference
        /// from when the request was originally created. This method will typically be called after creating a copy of
        /// a request that was originally created a long time ago, such as during scheduling.
        /// <param name="context">Request context identifying the current request.</param>
        /// <param name="timeDifference">The time difference.</param>
        /// </summary>
        DnsRequestTransaction TimeShift( IDnsRequestContext request, TimeSpan timeDifference );

        /// <summary>
        /// Allows a Local DataMart to process the request and optionally return one or more result documents
        /// </summary>
        /// <param name="request">Request context</param>
        /// <returns>Response containing a collection of response document on success or error messages on failure</returns>
        DnsResponseTransaction ExecuteRequest(IDnsRequestContext request);

        /// <summary>
        /// This method gets called every time a metadata response comes in. The plugin is supposed to parse the metadata
        /// documents and cache them for future use.
        /// </summary>
        void CacheMetadataResponse( Guid requestID, IDnsDataMartResponse response );

        /// <summary>
        /// Verifies that the given request is formed well enough to be submitted.
        /// </summary>
        DnsResult ValidateForSubmission( IDnsRequestContext context );


        /// <summary>
        /// Returns a list of aggregation modes formats that are supported for the given response.
        /// Null or empty return is treated as if exactly one mode is available.
        /// </summary>
        IEnumerable<IDnsResponseAggregationMode> GetAggregationModes( IDnsRequestContext context );

        IEnumerable<string> ValidateConfig(System.Collections.ArrayList arrayList);
    }

    public struct DnsRequestHeader
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IDnsActivity Activity { get; set; }
        public string ActivityDescription { get; set; }
        public Priorities? Priority { get; set; }
        public DateTime? DueDate { get; set; }

        public DateTime? Submitted { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public string Organization { get; set; }
    }

    public struct DnsRequestMetadata
    {
        public DnsRequestHeader Header { get; set; }
        public Func<IDnsDataMart, bool> DataMartFilter { get; set; }
    }

    public enum CommonSignatureFileTermType
    {
        MSReqID = RequestSearchTermType.MSReqID,
        MSProjID,
        MSWPType,
        MSWPID,
        MSVerID,
        RequestID,
        MP1Cycles,
        MP2Cycles,
        MP3Cycles,
        MP4Cycles,
        MP5Cycles,
        MP6Cycles,
        MP7Cycles,
        MP8Cycles,
        MP1Scenarios,
        MP2Scenarios,
        MP3Scenarios,
        MP4Scenarios,
        MP5Scenarios,
        MP6Scenarios,
        MP7Scenarios,
        MP8Scenarios,
        NumScen,
        NumCycle
    };

    public enum RequestSearchTermType
    {
        Text,
        ICD9DiagnosisCode,
        ICD9ProcedureCode,
        HCPCSCode,
        GenericDrugCode,
        DrugClassCode,
        SexStratifier,
        AgeStratifier,
        ClinicalSetting,
        ObservationPeriod,
        Coverage,
        OutputCriteria,
        MetricType,
        MSReqID,
        MSProjID,
        MSWPType,
        MSWPID,
        MSVerID,
        RequestID,
        MP1Cycles,
        MP2Cycles,
        MP3Cycles,
        MP4Cycles,
        MP5Cycles,
        MP6Cycles,
        MP7Cycles,
        MP8Cycles,
        MP1Scenarios,
        MP2Scenarios,
        MP3Scenarios,
        MP4Scenarios,
        MP5Scenarios,
        MP6Scenarios,
        MP7Scenarios,
        MP8Scenarios,
        NumScen,
        NumCycle,
        RequesterCenter,
        WorkplanType
    };

    public class DnsRequestSearchTerm 
    {
        public DnsRequestSearchTerm()
        {
            StringValue = null;
            NumberValue = null;
            DateFrom = null;
            DateTo = null;
            NumberFrom = null;
            NumberTo = null;
        }
        public int ID { get; set; }
        public Guid RequestID { get; set; }
        public RequestSearchTermType Type { get; set; }
        public string StringValue { get; set; }
        public decimal? NumberValue { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public decimal? NumberFrom { get; set; }
        public decimal? NumberTo { get; set; }
    }

    public struct DnsResponseTransaction
    {
        public IEnumerable<DocumentDTO> NewDocuments { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
        public bool IsFailed { get; set; }

        public static DnsResponseTransaction Failed(string errorMessage)
        {
            return new DnsResponseTransaction { IsFailed = true, ErrorMessages = new[] { errorMessage } };
        }
    }

    public struct DnsRequestTransaction
    {
        public IEnumerable<DocumentDTO> NewDocuments { get; set; }
        public IEnumerable<Document> RemoveDocuments { get; set; }
        public IDictionary<Document, IDnsDocument> UpdateDocuments { get; set; }
        public IEnumerable<DnsRequestSearchTerm> SearchTerms { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }
        public bool IsFailed { get; set; }

        public static DnsRequestTransaction Failed( string errorMessage )
        {
            return new DnsRequestTransaction { IsFailed = true, ErrorMessages = new[] { errorMessage } };
        }
    }
}