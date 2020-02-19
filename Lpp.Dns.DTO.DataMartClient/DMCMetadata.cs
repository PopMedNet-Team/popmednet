using System.Runtime.Serialization;

namespace Lpp.Dns.DTO.DataMartClient
{
    [DataContract]
    public class DMCMetadata
    {
        [DataMember]
        public string DMCFileVersion { get; set; }
        [DataMember]
        public string DMCProductVersion { get; set; }
        [DataMember]
        public string InvalidCredentials { get; set; }
    }
}
