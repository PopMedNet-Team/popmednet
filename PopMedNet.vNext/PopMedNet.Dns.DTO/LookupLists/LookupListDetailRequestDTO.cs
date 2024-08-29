using PopMedNet.Dns.DTO.Enums;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Lookup list for Detailed Request
    /// </summary>
    [DataContract]
    public class LookupListDetailRequestDTO
    {
        /// <summary>
        /// Available codes
        /// </summary>
        [DataMember]
        public IEnumerable<string> Codes { get; set; }
        /// <summary>
        /// List of id's
        /// </summary>
        [DataMember]
        public Lists ListID { get; set; }
    }}
