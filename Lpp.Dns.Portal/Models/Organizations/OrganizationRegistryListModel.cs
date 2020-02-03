using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.Portal.Models
{
    [DataContract]
    public class OrganizationRegistryListModel
    {
        [DataMember]
        public Guid OrganizationID { get; set; }
        [DataMember]
        public Guid RegistryID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
