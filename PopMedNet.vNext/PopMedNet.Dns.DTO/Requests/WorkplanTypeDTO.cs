using PopMedNet.Objects.ValidationAttributes;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// WorkPlan Type
    /// </summary>
    [DataContract]
    public class WorkplanTypeDTO
    {
        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        [DataMember]
        public Guid? ID { get; set; }
        /// <summary>
        /// Gets or set the Workplan Type ID
        /// </summary>
        [DataMember]
        public int WorkplanTypeID { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        [MaxLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// Gets or set the ID of network
        /// </summary>
        [DataMember]
        public Guid NetworkID { get; set; }
        /// <summary>
        /// Gets or set the acronym for the workplan type.
        /// </summary>
        [DataMember]
        public string Acronym { get; set; }
    }
}
