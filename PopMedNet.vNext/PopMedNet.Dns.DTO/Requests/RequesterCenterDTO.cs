using PopMedNet.Objects.ValidationAttributes;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Request center
    /// </summary>
    [DataContract]
    public class RequesterCenterDTO
    {
        /// <summary>
        /// Gets or sets the ID of center
        /// </summary>
        [DataMember]
        public Guid? ID { get; set; }
        /// <summary>
        /// Returns ID
        /// </summary>
         [DataMember]
        public int RequesterCenterID { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        [MaxLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the ID of network
        /// </summary>
        [DataMember]
        public Guid NetworkID { get; set; }
    }
}
