using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// DataMart Events
    /// </summary>
    [DataContract]
    public class DataMartEventDTO : BaseEventPermissionDTO
    {
        /// <summary>
        /// The ID of the DataMart
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
    }
}
