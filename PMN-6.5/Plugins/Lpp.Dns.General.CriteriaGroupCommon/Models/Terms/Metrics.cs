using System.Runtime.Serialization;

namespace RequestCriteria.Models
{
    [DataContract]
    public class MetricsData : TermData
    {
        [DataMember]
        public MetricsTermTypes MetricsTermType { get; set; }

        [DataMember]
        public MetricsTypes[] Metrics { get; set; }
    }

    public enum MetricsTermTypes
    {
        [EnumMember]
        Race = 0,
        [EnumMember]
        Ethnicity = 1,
        [EnumMember]
        Diagnoses = 2,
        [EnumMember]
        Procedures = 3,
        [EnumMember]
        NDC = 4
    }

    public enum MetricsTypes
    {
        [EnumMember]
        DataPartnerCount = 0,
        [EnumMember]
        DataPartnerPercent = 1,
        [EnumMember]
        DataPartnerPercentContribution = 2,
        [EnumMember]
        DataPartnerPresence = 3,
        [EnumMember]
        Overall = 4,
        [EnumMember]
        OverallCount = 5, 
        [EnumMember]
        OverallPresence = 6 
    }
}
