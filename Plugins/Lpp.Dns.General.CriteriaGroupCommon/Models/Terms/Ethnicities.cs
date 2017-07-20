using System.Runtime.Serialization;

namespace RequestCriteria.Models
{
    [DataContract]
    public class EthnicityData : TermData
    {
        [DataMember]
        public EthnicityTypes[] Ethnicities { get; set; }
    }

    public enum EthnicityTypes
    {
        [EnumMember]
        Hispanic = 0,
        [EnumMember]
        NotHispanic = 1,
        [EnumMember]
        Unknown = 2, 
        [EnumMember]
        Missing = 3
    }
}
