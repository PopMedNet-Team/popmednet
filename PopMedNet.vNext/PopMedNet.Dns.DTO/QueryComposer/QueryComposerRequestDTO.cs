using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.QueryComposer
{
    /// <summary>
    /// Query composer Request DTO
    /// </summary>
    [DataContract]
    public class QueryComposerRequestDTO
    {
        public QueryComposerRequestDTO()
        {
            SchemaVersion = "2.0";
        }
        /// <summary>
        /// Gets or sets the schema version of the request DTO.
        /// </summary>
        [DataMember]
        public string SchemaVersion { get; private set; }
        /// <summary>
        /// Gets or sets the header for the request DTO.
        /// </summary>
        [DataMember]
        public QueryComposerRequestHeaderDTO Header { get; set; }

        /// <summary>
        /// Gets or sets the collection of queries for the request.
        /// </summary>
        [DataMember]
        public IEnumerable<QueryComposerQueryDTO> Queries { get; set; }

        /// <summary>
        /// Sets all the query headers to match the DueDate, Priority, SubmittedOn, and ViewUrl specified in the request header.
        /// </summary>
        public void SyncHeaders()
        {
            foreach (var query in Queries)
            {
                query.Header.DueDate = Header.DueDate;
                query.Header.Priority = Header.Priority;
                query.Header.SubmittedOn = Header.SubmittedOn;
                query.Header.ViewUrl = Header.ViewUrl;
            }
        }
    }
}
