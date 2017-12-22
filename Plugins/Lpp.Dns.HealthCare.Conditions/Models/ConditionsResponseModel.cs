using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Lpp.Dns.HealthCare.Conditions.Models
{
    [DataContract]
    public class ConditionsResponseModel
    {
        [DataMember]
        public IEnumerable<string> Headers { get; set; }

        [DataMember]
        public DataSet RawData { get; set; }

        [DataMember]
        public bool Aggregated { get; set; }

        [DataMember]
        public bool Projected { get; set; }
    }
}