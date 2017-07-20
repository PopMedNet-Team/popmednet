using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    [DataContract]
    public enum PatientEncounterTypes
    {
        [EnumMember, Description("Unknown")]
        Unknown = 0,
        [EnumMember, Description("Ambulatory Visit")]
        Ambulatory = 1,
        [EnumMember, Description("Emergency Department")]
        Emergency = 2,
        [EnumMember, Description("Emergency Department Admit to Inpatient Hospital Stay")]
        EmergencyAdmit = 8,
        [EnumMember, Description("Inpatient Hospital Stay")]
        Inpatient = 3,
        [EnumMember, Description("Non-Acute Institutional Stay")]
        Institutional = 4,
        [EnumMember, Description("Other Ambulatory Visit")]
        OtherAmbulatory = 5,
        [EnumMember, Description("Other")]
        Other = 6,
        [EnumMember, Description("No Information")]
        NoInformation = 7       
    }
}
