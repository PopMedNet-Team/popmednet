using System.Runtime.Serialization;

namespace RequestCriteria.Models
{
    [DataContract]
    public class AgeStratifierData : TermData
    {
        [DataMember]
        public AgeStratifierTypes AgeStratifier { get; set; }
    }

    public enum AgeStratifierTypes
    {
        [EnumMember]
        NotSpecified = -1,
        [EnumMember]
        None = 0,
        [EnumMember]
        Ten = 1,
        [EnumMember]
        Seven = 2,
        [EnumMember]
        Four = 3,
        [EnumMember]
        Two = 4
    }
}
