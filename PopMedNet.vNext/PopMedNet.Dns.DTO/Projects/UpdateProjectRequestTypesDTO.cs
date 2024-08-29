using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Update Project request types
    /// </summary>
    [DataContract]
    public class UpdateProjectRequestTypesDTO
    {
        /// <summary>
        /// Gets or set the ID of project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
        /// <summary>
        /// Available Request types in Project
        /// </summary>
        [DataMember]
        public IEnumerable<ProjectRequestTypeDTO>? RequestTypes { get; set; }
    }
}
