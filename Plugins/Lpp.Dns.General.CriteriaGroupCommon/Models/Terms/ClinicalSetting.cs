using System.Runtime.Serialization;

namespace RequestCriteria.Models
{
    [DataContract]
    public class ClinicalSettingData : TermData
    {
        [DataMember]
        public ClinicalSettingTypes ClinicalSetting { get; set; }
    }

    [DataContract]
    public enum ClinicalSettingTypes
    {
        [EnumMember]
        NotSpecified = -1,
        [EnumMember]
        Any = 0,
        [EnumMember]
        InPatient = 1,
        [EnumMember]
        OutPatient = 2,
        [EnumMember]
        Emergency = 3
    }
}
