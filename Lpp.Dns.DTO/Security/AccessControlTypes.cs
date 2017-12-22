using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Access control Types
    /// </summary>
    [DataContract]
    public enum AccessControlTypes
    {
        /// <summary>
        /// Read
        /// </summary>
        [EnumMember]
        Read = 0,
        /// <summary>
        /// Insert
        /// </summary>
        [EnumMember]
        Insert = 1,
        /// <summary>
        /// Update
        /// </summary>
        [EnumMember]
        Update = 2,
        /// <summary>
        /// Delete
        /// </summary>
        [EnumMember]
        Delete = 3,
        /// <summary>
        /// List Security Groups
        /// </summary>
        [EnumMember, Description("List Security Groups")]
        SecurityGroupRead = 10,
        /// <summary>
        /// Add Security Groups
        /// </summary>
        [EnumMember, Description("Add Security Groups")]
        SecurityGroupInsert = 11,
        /// <summary>
        /// Update Security Groups
        /// </summary>
        [EnumMember, Description("Update Security Groups")]
        SecurityGroupUpdate = 12,
        /// <summary>
        /// Delete Security Groups
        /// </summary>
        [EnumMember, Description("Delete Security Groups")]
        SecurityGroupDelete = 13,
        /// <summary>
        /// Manage security Group users
        /// </summary>

        [EnumMember, Description("Manage Security Group Users")]
        SecurityGroupManageUsers = 20,
       
              
        //Queries
        /// <summary>
        /// Submit Queries
        /// </summary>
        [EnumMember, Description("Submit Queries")]
        QueriesSubmit = 1000,
        /// <summary>
        /// SAve Queries
        /// </summary>
        [EnumMember, Description("Save Queries")]
        QueriesSave = 1001,
        /// <summary>
        /// Copy Queries
        /// </summary>
        [EnumMember, Description("Copy Queries")]
        QueriesCopy = 1002,
        /// <summary>
        /// List Queries
        /// </summary>
        [EnumMember, Description("List Queries")]
        QueriesRead = 1003,
    }
}
