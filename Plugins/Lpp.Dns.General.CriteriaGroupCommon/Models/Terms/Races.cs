using System.Runtime.Serialization;

namespace RequestCriteria.Models
{
    [DataContract]
    public class RaceData : TermData
    {
        [DataMember]
        public RaceTypes[] Races { get; set; }
    }

    public enum RaceTypes
    {
        [EnumMember]
        Unknown = 0,
        [EnumMember]
        AmericanIndianOrAlaskaNative = 1,
        [EnumMember]
        Asian = 2,
        [EnumMember]
        BlackOrAfricanAmerican = 3,
        [EnumMember]
        NativeHawaiianOrOtherPacificIslander = 4,
        [EnumMember]
        White = 5,
        [EnumMember]
        Missing = 6
    }
}
