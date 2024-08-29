using PopMedNet.Objects;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Response Group
    /// </summary>
    [DataContract]
    public class ResponseGroupDTO : EntityDtoWithID
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
}
