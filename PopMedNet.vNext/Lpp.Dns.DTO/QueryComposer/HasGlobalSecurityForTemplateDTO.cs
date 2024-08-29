using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO
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
