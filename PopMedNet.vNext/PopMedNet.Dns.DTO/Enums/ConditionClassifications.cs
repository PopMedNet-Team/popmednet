using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Conditions
    /// </summary>
    [DataContract]
    public enum ConditionClassifications
    {
        /// <summary>
        /// Indicates condition is Influenza like Illness
        /// </summary>
        [EnumMember, Description("Influenza-like Illness")]
        Influenza = 1,
        /// <summary>
        /// Indicates condition is Type1 Diabetes
        /// </summary>
        [EnumMember, Description("Diabetes: Type 1")]
        Type1Diabetes = 20,
        /// <summary>
        /// Indicates condition is Type2 Diabetes
        /// </summary>
        [EnumMember, Description("Diabetes: Type 2")]
        Type2Diabetes = 21,
        /// <summary>
        /// Indicates condition is Gestational Diabetes
        /// </summary>
        [EnumMember, Description("Diabetes: Gestational Diabetes")]
        GestationalDiabetes = 22,
        /// <summary>
        /// Indicates condition is Prediabetes
        /// </summary>
        [EnumMember, Description("Diabetes: Prediabetes")]
        Prediabetes = 23,
        /// <summary>
        ///Indicates condition is Asthma
        /// </summary>
        [EnumMember, Description("Asthma")]
        Asthma = 30,
        /// <summary>
        ///Indicates condition is Depression
        /// </summary>
        [EnumMember, Description("Depression")]
        Depression = 35,
        /// <summary>
        ///Indicates condition is Opioid Prescription
        /// </summary>
        [EnumMember, Description("Opioid Prescription")]
        opioidrx = 40,
        /// <summary>
        ///Indicates condition is Benzodiazepine Prescription
        /// </summary>
        [EnumMember, Description("Benzodiazepine Prescription")]
        benzodiarx = 41,
        /// <summary>
        ///Indicates condition is Concurrent Benzodiazepine-Opioid Prescription
        /// </summary>
        [EnumMember, Description("Concurrent Benzodiazepine-Opioid Prescription")]
        benzopiconcurrent = 42,
        /// <summary>
        ///Indicates condition is High Opioid Use
        /// </summary>
        [EnumMember, Description("High Opioid Use")]
        highopioiduse = 43
    }
}
