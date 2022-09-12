using Lpp.Objects.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Users
{
    [DataContract]
    public class EmailTemplateSubstitutionPropertiesDTO
    {
        [DataMember]
        public Guid? UserID { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember, EmailAddress, MaxLength(255)]
        public string EmailAddress { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public int TemplateTypeID { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string Network { get; set; }
        [DataMember]
        public string NetworkUrl { get; set; }
        [DataMember]
        public string QueryToolName { get; set; }
        [DataMember]
        public string ApiUrl { get; set; }
    }
}
