using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Project Events
    /// </summary>
    [DataContract]
    public class ProjectEventDTO : BaseEventPermissionDTO
    {
        /// <summary>
        /// ID of the Project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
    }
}
