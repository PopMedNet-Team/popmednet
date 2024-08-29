using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Task Items
    /// </summary>
    [DataContract]
    public enum TaskItemTypes
    {
        /// <summary>
        /// Indicates the task reference is to a User.
        /// </summary>
        [EnumMember]
        User = 1,
        /// <summary>
        /// Indicates the task reference is to a Request.
        /// </summary>
        [EnumMember]
        Request = 2,
        /// <summary>
        /// Indicates the task reference is to the root document that contains data for task that is needed by the screeen.
        /// </summary>
        [EnumMember]
        ActivityDataDocument = 3,
        /// <summary>
        /// Indicates the task reference is to a response.
        /// </summary>
        [EnumMember]
        Response = 4,
        /// <summary>
        /// Indicates the task reference is to a response that should have aggregation applied.
        /// </summary>
        [EnumMember]
        AggregateResponse = 5,
        /// <summary>
        /// Indicates the task reference is to a Query Template.
        /// </summary>
        [EnumMember]
        QueryTemplate = 6,
        /// <summary>
        /// Indicates the task reference is to a Request Type.
        /// </summary>
        [EnumMember]
        RequestType = 7,
        /// <summary>
        /// Indicates the task reference is to a Project.
        /// </summary>
        [EnumMember]
        Project = 8,
        /// <summary>
        /// Indicates the task reference is to a a Request Attachment
        /// </summary>
        [EnumMember]
        RequestAttachment = 9
    }
}
