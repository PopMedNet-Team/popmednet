using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.General.Metadata.Models
{
    [DataContract]
    public class MetadataDataMartResponseModel
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Acronym { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string HealthPlan { get; set; }
    }
}
