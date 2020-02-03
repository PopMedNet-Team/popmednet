using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.General.Metadata.Models
{
    [DataContract]
    public class MetadataOrgResponseModel
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Guid? ParentID { get; set; }
        [DataMember]
        public string ParentName { get; set; }
        [DataMember]
        public string Acronym { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string ContactName { get; set; }
        [DataMember]
        public string HealthPlanDescription { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
