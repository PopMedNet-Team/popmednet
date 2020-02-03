using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

// TODO: Change this to conform with PMN standard e.g. Lpp.Dns.HealthCare.RequestCriteria.Models
// TODO: If this is shared between data checker and metadata plugin, factor it into a separate shared folder/namespace
namespace RequestCriteria.Models
{
    [DataContract]
    public class RequestCriteriaData
    {
        [DataMember]
        public Guid RequestType { get; set; }

        [DataMember]
        public IEnumerable<CriteriaData> Criterias { get; set; }   
    }
}
