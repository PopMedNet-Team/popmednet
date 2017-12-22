using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Lpp.Dns.Portal.Models
{
    [DataContract]
    public class EditAclModel
    {
        [DataMember]
        public string ParentViewModelProperty { get; set; }
    }
}