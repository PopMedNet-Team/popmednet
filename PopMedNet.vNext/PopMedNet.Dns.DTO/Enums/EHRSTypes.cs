using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Types of EHRS
    /// </summary>
    [DataContract]
    public enum EHRSTypes
    {
        /// <summary>
        /// Indicates EHR type is Inpatient
        /// </summary>
        [EnumMember]
        Inpatient = 1,
        /// <summary>
        /// Indicates EHR type is Outpatient
        /// </summary>
        [EnumMember]
        Outpatient = 2
    }
}
