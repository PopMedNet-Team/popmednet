using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RequestCriteria.Models
{
    [DataContract]
    public class EncounterData : TermData
    {
        [DataMember]
        public EncounterTypes[] Encounters { get; set; }
    }

    [DataContract]
    public enum EncounterTypes
    {
        [EnumMember]
        All = 0,
        [EnumMember]
        AmbulatoryVisit = 1,
        [EnumMember]
        EmergencyDepartment = 2,
        [EnumMember]
        InpatientHospitalStay = 3,
        [EnumMember]
        NonAcuteInstitutionalStay = 4,
        [EnumMember]
        OtherAmbulatoryVisit = 5,
        [EnumMember]
        Missing = 6,
    }
}
