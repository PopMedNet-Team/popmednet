using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Comment of item Type
    /// </summary>
    [DataContract]
    public enum CommentItemTypes
    {
        /// <summary>
        ///Indicates Comment item type is Task
        /// </summary>
        [EnumMember]
        Task = 1,
        /// <summary>
        /// Indicates Comment item type is Document
        /// </summary>
        [EnumMember]
        Document = 2
    }
}
