using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    [DataContract]
    public class CodeSelectorValueDTO
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime? ExpireDate { get; set; }
    }
}
