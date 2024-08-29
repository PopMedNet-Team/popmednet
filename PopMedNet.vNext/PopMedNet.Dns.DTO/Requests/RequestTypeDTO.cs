using PopMedNet.Objects;
using PopMedNet.Objects.ValidationAttributes;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Request Type
    /// </summary>
    [DataContract]
    public class RequestTypeDTO : EntityDtoWithID
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        [Required, MaxLength(255)]
        public string? Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string? Description { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if request metadata is filled
        /// </summary>
        [DataMember]
        public bool Metadata { get; set; }
        /// <summary>
        /// post process
        /// </summary>
        [DataMember]
        public bool PostProcess { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if files are added
        /// </summary>
        [DataMember]
        public bool AddFiles { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if request type requires processsing
        /// </summary>
        [DataMember]
        public bool RequiresProcessing { get; set; }
        /// <summary>
        /// Gets or sets the notes for the request type.
        /// </summary>
        [DataMember]
        public string? Notes { get; set; }
        /// <summary>
        /// Gets or sets the ID of workflow
        /// </summary>
        [DataMember]
        public Guid? WorkflowID { get; set; }
        /// <summary>
        /// Workflow
        /// </summary>
        [DataMember]
        public string? Workflow { get; set; }
        /// <summary>
        /// Gets or sets if the request type supportes multi-query.
        /// </summary>
        [DataMember]
        public bool SupportMultiQuery { get; set; }
    }
}
