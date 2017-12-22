using System;
using System.Runtime.Serialization;

namespace RequestCriteria.Models
{
    [DataContract]
    public class ReportAggregationLevelData : TermData
    {
        [DataMember]
        public Guid ReportAggregationLevelID { get; set; }
    }
}
