using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Objects;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// DTO for referencing Term
    /// </summary>
    [DataContract]
    public class TermDTO : EntityDtoWithID
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember, PopMedNet.Objects.ValidationAttributes.MaxLength(255)]
        public string? Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string? Description { get; set; }
        /// <summary>
        /// OID
        /// </summary>
        [DataMember, PopMedNet.Objects.ValidationAttributes.MaxLength(100)]
        public string? OID { get; set; }
        /// <summary>
        /// ReferenceUrl
        /// </summary>
        [DataMember, PopMedNet.Objects.ValidationAttributes.MaxLength(450)]
        public string? ReferenceUrl { get; set; }
        /// <summary>
        /// Gets or sets the type of Term types
        /// </summary>
        [DataMember]
        public TermTypes Type { get; set; }
    }
}
