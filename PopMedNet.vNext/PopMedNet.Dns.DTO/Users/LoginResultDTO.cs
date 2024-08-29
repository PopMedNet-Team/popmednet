using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Dns.DTO
{
    [DataContract]
    public class LoginResultDTO
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string UserName { get; set; } = string.Empty;
        [DataMember]
        public string FullName { get; set; } = string.Empty;
        [DataMember]
        public Guid? OrganizationID { get; set; }
        [DataMember]
        public DateTime? PasswordExpiration { get; set; }
    }
}
