using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    [DataContract]
    public class HasGlobalSecurityForTemplateDTO
    {
        [DataMember]
        public bool SecurityGroupExistsForGlobalPermission { get; set; }
        [DataMember]
        public bool CurrentUserHasGlobalPermission { get; set; }

    }
}
