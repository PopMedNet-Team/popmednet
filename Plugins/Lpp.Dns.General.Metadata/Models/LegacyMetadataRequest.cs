using RequestCriteria.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

// TODO: Change this to conform with PMN standard e.g. Lpp.Dns.HealthCare.RequestCriteria.Models
// TODO: If this is shared between data checker and metadata plugin, factor it into a separate shared folder/namespace
namespace Lpp.Dns.General.Metadata.Models
{
    [DataContract]
    public class LegacyMetadataRequestData : RequestCriteriaData
    {
        [DataMember]
        public int? TaskOrder { get; set; }

        [DataMember]
        public int? Activity { get; set; }

        [DataMember]
        public int? ActivityProject { get; set; }

        [DataMember]
        public int? SourceTaskOrder { get; set; }

        [DataMember]
        public int? SourceActivity { get; set; }

        [DataMember]
        public int? SourceActivityProject { get; set; }
    }
}
