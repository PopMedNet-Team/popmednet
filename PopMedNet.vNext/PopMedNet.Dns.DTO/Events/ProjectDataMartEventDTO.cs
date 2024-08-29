using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Project DataMart Events
    /// </summary>
    [DataContract]
    public class ProjectDataMartEventDTO : BaseEventPermissionDTO
    {
        /// <summary>
        /// ID of the Project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
        /// <summary>
        /// ID of DataMart
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
    }
}
