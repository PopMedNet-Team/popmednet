using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Security Group kinds
    /// </summary>
    [DataContract]
    public enum SecurityGroupKinds
    {
        /// <summary>
        /// Indicates SecurityGroup kind is Custom
        /// </summary>
        [EnumMember, Description("Custom")]
        Custom = 0,
        /// <summary>
        /// Indicates SecurityGroup kind is Everyone
        /// </summary>
        [EnumMember, Description("Everyone")]
        Everyone = 1,
        /// <summary>
        /// Indicates SecurityGroup kind is Administrators
        /// </summary>
        [EnumMember, Description("Administrators")]
        Administrators = 2,
        /// <summary>
        /// Indicates SecurityGroup kind is Investigators
        /// </summary>
        [EnumMember, Description("Investigators")]
        Investigators = 3,
        /// <summary>
        /// Indicates SecurityGroup kind is Enhanced Investigators
        /// </summary>
        [EnumMember, Description("Enhanced Investigators")]
        EnhancedInvestigators = 4,
        /// <summary>
        /// Indicates SecurityGroup kind is Query Administrators
        /// </summary>
        [EnumMember, Description("Query Administrators")]
        QueryAdministrators = 5,
        /// <summary>
        /// Indicates SecurityGroup kind is DataMart Administrators
        /// </summary>
        [EnumMember, Description("DataMart Administrators")]
        DataMartAdministrators = 6,
        /// <summary>
        /// Indicates SecurityGroup kind is Observers
        /// </summary>
        [EnumMember, Description("Observers")]
        Observers = 7,
        /// <summary>
        /// Indicates SecurityGroup kind is User
        /// </summary>
        [EnumMember, Description("User")]
        Users = 8,
        /// <summary>
        /// Indicates SecurityGroup kind is Group DataMart Administrator
        /// </summary>
        [EnumMember, Description("Group DataMart Administrator")]
        GroupDataMartAdministrator = 9
    }
}
