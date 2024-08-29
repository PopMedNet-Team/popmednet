using PopMedNet.Objects;
using PopMedNet.Objects.ValidationAttributes;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Project
    /// </summary>
    [DataContract]
    public class ProjectDTO : EntityDtoWithID
    {
        /// <summary>
        /// Gets or sets the project name
        /// </summary>
        [DataMember]
        [MaxLength(255), Required]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the project acronym
        /// </summary>
        [DataMember]
        [MaxLength(50)]
        public string? Acronym { get; set; }
        /// <summary>
        /// Gets or sets the start date 
        /// </summary>
        [DataMember]
        public DateTimeOffset? StartDate { get; set; }
        /// <summary>
        /// Gets or sets the project end date
        /// </summary>
        [DataMember]
        public DateTimeOffset? EndDate { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if deleted
        /// </summary>
        [DataMember]
        public bool Deleted { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if activated
        /// </summary>
        [DataMember]
        public bool Active { get; set; }
        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [DataMember]
        public string? Description { get; set; }
        /// <summary>
        /// Gets or sets the id of a group
        /// </summary>
        [DataMember, Newtonsoft.Json.JsonProperty]
        public Guid? GroupID { get; set; }
        /// <summary>
        /// Gets or sets the Group
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string? Group { get; set; }
    }
}
