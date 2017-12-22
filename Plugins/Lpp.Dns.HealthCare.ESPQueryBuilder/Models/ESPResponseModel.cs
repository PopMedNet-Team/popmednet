using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder.Models
{
    [DataContract]
    public class ESPResponseModel
    {
        [DataMember]
        public IEnumerable<string> Headers { get; set; }

        [DataMember]
        public DataSet RawData { get; set; }

        [DataMember]
        public bool Aggregated { get; set; }

        [DataMember]
        public bool Projected { get; set; }

        [DataMember]
        public bool StratifyProjectedViewByAgeGroup { get; set; }

        [DataMember]
        public bool StratificationIncludesLocations { get; set; }

        [DataMember]
        public IEnumerable<PredefinedLocationItem> Locations { get; set; }
    }
}