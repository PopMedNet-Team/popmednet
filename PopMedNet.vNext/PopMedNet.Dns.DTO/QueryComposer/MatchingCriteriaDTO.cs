using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.QueryComposer
{
    /// <summary>
    /// Query composer matching criteria
    /// </summary>
    [DataContract]
    public class MatchingCriteriaDTO
    {
        /// <summary>
        /// Available Terms
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> TermIDs { get; set; }
        /// <summary>
        /// Gets or sets the ID of project
        /// </summary>
        [DataMember]
        public Guid? ProjectID { get; set; }

        /// <summary>
        /// The Request json to use when retrieving datamarts capable of responding.
        /// </summary>
        [DataMember]
        public string Request { get; set; }

        /// <summary>
        /// Gets or sets the ID of the Request
        /// </summary>
        [DataMember]
        public Guid? RequestID { get; set; }

        //Add the stratifiers etc. here as we need them.
    }
}
