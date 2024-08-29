using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Objects.ValidationAttributes;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Lookup List Values
    /// </summary>
    [DataContract]
    public class LookupListValueDTO
    {
        /// <summary>
        /// List of id's
        /// </summary>
        [DataMember]
        public Lists ListId { get; set; }
        /// <summary>
        /// Return Category id
        /// </summary>
        [DataMember]
        public int CategoryId { get; set; }
        /// <summary>
        /// Item Name
        /// </summary>
        [DataMember]
        [MaxLength(500)]
        public string ItemName { get; set; }
        /// <summary>
        /// Item Code
        /// </summary>
        [DataMember]
        [MaxLength(200)]
        public string ItemCode { get; set; }
        /// <summary>
        /// Item code with No Period
        /// </summary>
        [DataMember]
        [MaxLength(200)]
        public string ItemCodeWithNoPeriod { get; set; }
        /// <summary>
        /// Item code Expiration date
        /// </summary>
        [DataMember]
        public DateTime? ExpireDate { get; set; }
        /// <summary>
        /// Returns ID
        /// </summary>
        [DataMember]
        public int ID { get; set; }
    }
}
