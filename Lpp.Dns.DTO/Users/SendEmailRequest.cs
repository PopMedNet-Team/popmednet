using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Users
{
    [DataContract]
    public class SendEmailRequest
    {
        [DataMember(IsRequired = true)]
        public EmailTemplateSubstitutionPropertiesDTO Properties { get; set; }
        [DataMember(IsRequired = true)]
        public string HtmlContent { get; set; }
        [DataMember(IsRequired = true)]
        public string TextContent { get; set; }
    }
}
