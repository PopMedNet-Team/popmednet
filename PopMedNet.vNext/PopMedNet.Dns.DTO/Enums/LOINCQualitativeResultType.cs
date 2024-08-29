using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Qualitative Results for LOIC Lab Records
    /// </summary>
    [DataContract]
    public enum LOINCQualitativeResultType
    {
        /// <summary>
        /// Positive
        /// </summary>
        [EnumMember, Description("Positive")]
        Positive = 1,
        /// <summary>
        /// Negative
        /// </summary>
        [EnumMember, Description("Negative")]
        Negative = 2,
        /// <summary>
        /// Borderline
        /// </summary>
        [EnumMember, Description("Borderline")]
        Borderline = 3,
        /// <summary>
        /// Elevated
        /// </summary>
        [EnumMember, Description("Elevated")]
        Elevated = 4,
        /// <summary>
        /// High
        /// </summary>
        [EnumMember, Description("High")]
        High = 5,
        /// <summary>
        /// Low
        /// </summary>
        [EnumMember, Description("Low")]
        Low = 6,
        /// <summary>
        /// Normal
        /// </summary>
        [EnumMember, Description("Normal")]
        Normal = 7,
        /// <summary>
        /// Abnormal
        /// </summary>
        [EnumMember, Description("Abnormal")]
        Abnormal = 8,
        /// <summary>
        /// Undetermined
        /// </summary>
        [EnumMember, Description("Undetermined")]
        Undetermined = 9,
        /// <summary>
        /// Undetectable
        /// </summary>
        [EnumMember, Description("Undetectable")]
        Undetectable = 10,
        /// <summary>
        /// NI
        /// </summary>
        [EnumMember, Description("NI")]
        NI = 11,
        /// <summary>
        /// UN
        /// </summary>
        [EnumMember, Description("UN")]
        UN = 12,
        /// <summary>
        /// OT
        /// </summary>
        [EnumMember, Description("OT")]
        OT = 13,
        /// <summary>
        /// Detected
        /// </summary>
        [EnumMember, Description("Detected")]
        Detected = 14,
        /// <summary>
        /// Equivocal
        /// </summary>
        [EnumMember, Description("Equivocal")]
        Equivocal = 15,
        /// <summary>
        /// Indeterminate abnormal
        /// </summary>
        [EnumMember, Description("Indeterminate abnormal")]
        Indeterminate_Abnormal = 16,
        /// <summary>
        /// Invalid
        /// </summary>
        [EnumMember, Description("Invalid")]
        Invalid = 17,
        /// <summary>
        /// Nonreactive
        /// </summary>
        [EnumMember, Description("Nonreactive")]
        Nonreactive = 18,
        /// <summary>
        /// Not Detected
        /// </summary>
        [EnumMember, Description("Not Detected")]
        NotDetencted = 19,
        /// <summary>
        /// Past Infection
        /// </summary>
        [EnumMember, Description("Past Infection")]
        PastInfected = 20,
        /// <summary>
        /// Presumptive Positive
        /// </summary>
        [EnumMember, Description("Presumptive Positive")]
        PresumptivePositive = 21,
        /// <summary>
        /// Reactive
        /// </summary>
        [EnumMember, Description("Reactive")]
        Reactive = 22,
        /// <summary>
        /// Recent Infection
        /// </summary>
        [EnumMember, Description("Recent Infection")]
        RecentInfection = 23,
        /// <summary>
        /// Specimen Unsatisfactory
        /// </summary>
        [EnumMember, Description("Specimen Unsatisfactory")]
        SpecimenUnsatisfactory = 24,
        /// <summary>
        /// Suspected
        /// </summary>
        [EnumMember, Description("Suspected")]
        Suspected = 25
    }
}
