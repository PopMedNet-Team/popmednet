using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
//using Lpp.Dns.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder.Data
{
    public class ESPCensusDataSelection
    {
        public ProjectionType ProjectionType { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Town { get; set; }
        public string Region { get; set; }
        public Stratifications Stratification { get; set; }
        public bool StratifyProjectedViewByAgeGroup { get; set; }
    }

    [DataContract]
    public enum ProjectionType
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        PopulationProjection = 1,
        [EnumMember]
        GeographicProjection = 2
    }
}
