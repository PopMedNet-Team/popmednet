using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.QueryComposer
{
    /// <summary>
    /// Querycomposer Response DTO
    /// </summary>
    [DataContract]
    public class QueryComposerResponseDTO
    {
        /// <summary>
        /// Gets the schema version of the response.
        /// </summary>
        [DataMember]
        public string SchemaVersion { get { return "2.0";  } }
        /// <summary>
        /// Gets or sets the header for the response.
        /// </summary>
        [DataMember]
        public QueryComposerResponseHeaderDTO Header { get; set; }
        /// <summary>
        /// Gets or sets the collection of errors that occured while processing the queries.
        /// </summary>
        [DataMember]
        public IEnumerable<QueryComposerResponseErrorDTO> Errors { get; set; }
        /// <summary>
        /// Gets or sets the query responses for the request.
        /// </summary>
        [DataMember]
        public IEnumerable<QueryComposerResponseQueryResultDTO> Queries { get; set; }

        /// <summary>
        /// Refreshes the response query start and end dates based on the first query result start date, and the last query result finish or post-processing finish date.
        /// If there are no queries, the query start and end dates are set to null.
        /// </summary>
        public void RefreshQueryDates()
        {
            if (Header == null)
                return;

            if(Queries == null)
            {
                Header.QueryingStart = null;
                Header.QueryingEnd = null;
                return;
            }

            Header.QueryingStart = Queries.Where(q => q.QueryStart.HasValue).Max(q => (DateTimeOffset?)q.QueryStart.Value);
            Header.QueryingEnd = Queries.Where(q => q.QueryEnd.HasValue).Select(q => q.QueryEnd.Value).Concat(Queries.Where(q => q.PostProcessEnd.HasValue).Select(q => q.PostProcessEnd.Value)).Max(d => (DateTimeOffset?)d);
        }

        /// <summary>
        /// Combines all response and query result errors into a single collection.
        /// If there are no errors the collection will be null.
        /// </summary>
        public void RefreshErrors()
        {
            Errors = (Errors ?? Enumerable.Empty<QueryComposerResponseErrorDTO>()).Where(e => e.QueryID == null).Concat((Queries ?? Enumerable.Empty<QueryComposerResponseQueryResultDTO>()).Where(q => q.Errors != null).SelectMany(q => q.Errors)).ToArray();

            if (((QueryComposerResponseErrorDTO[])Errors).Length == 0)
            {
                Errors = null;
            }
        }
    }
    
}
