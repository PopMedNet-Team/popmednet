using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Group Events
    /// </summary>
    [DataContract]
    public class GroupEventDTO : BaseEventPermissionDTO
    {
        /// <summary>
        /// ID of Group
        /// </summary>
        [DataMember]
        public Guid GroupID { get; set; }
    }
}
