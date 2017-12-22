using System.Runtime.Serialization;

namespace RequestCriteria.Models
{
    [DataContract]
    public class SexData : TermData
    {
        [DataMember]
        public SexTypes Sex { get; set; }
    }

    [DataContract]
    public enum SexTypes
    {
        [EnumMember]
        NotSpecified = -1,
        [EnumMember]
        Male = 1,
        [EnumMember]
        Female = 2,
        [EnumMember]
        Both = 3,
        [EnumMember]
        Aggregated = 4,
    }
}
