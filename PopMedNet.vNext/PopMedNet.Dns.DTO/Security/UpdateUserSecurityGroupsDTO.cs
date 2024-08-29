using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// User Security Groups update
    /// </summary>
    [DataContract]
    public class UpdateUserSecurityGroupsDTO
    {
        /// <summary>
        /// Gets or sets the ID of the user
        /// </summary>
        [DataMember]
        public Guid UserID { get; set; }
        /// <summary>
        /// list of Groups
        /// </summary>
        [DataMember]
        public IEnumerable<SecurityGroupDTO> Groups { get; set; }
    }
}
