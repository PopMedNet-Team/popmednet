using PopMedNet.Dns.DTO.Enums;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.QueryComposer
{
    /// <summary>
    /// Query composer OrderBy
    /// </summary>
    [DataContract]
    public class QueryComposerOrderByDTO
    {
        /// <summary>
        /// Perform OrderBy Directions
        /// </summary>
        [DataMember]
        public OrderByDirections Direction { get; set; }
    }
}
