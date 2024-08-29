using System.Runtime.Serialization;

namespace PopMedNet.DMCS.PMNApi.DTO
{
    [DataContract]
    public class LoginDTO
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public bool RememberMe { get; set; }
        [DataMember]
        public string IPAddress { get; set; }
        [DataMember]
        public string Enviorment { get; set; }
    }
}
