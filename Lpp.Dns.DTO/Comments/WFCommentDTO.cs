using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// A workflow comment.
    /// </summary>
    [DataContract]
    public class WFCommentDTO : EntityDtoWithID
    {
        /// <summary>
        /// The comment content.
        /// </summary>
        [DataMember]
        public string Comment { get; set; }
        /// <summary>
        /// The date the comment was created on.
        /// </summary>
        [DataMember]
        public DateTimeOffset CreatedOn { get; set; }
        /// <summary>
        /// The ID of the user that created the comment.
        /// </summary>
        [DataMember]
        public Guid CreatedByID { get; set; }
        /// <summary>
        /// The username of the person that created the comment.
        /// </summary>
        [DataMember]
        public string CreatedBy { get; set; }
        /// <summary>
        /// The ID of the request the comment is associated with.
        /// </summary>
        [DataMember]
        public Guid RequestID { get; set; }
        /// <summary>
        /// The ID of the task associated to the comment.
        /// </summary>
        [DataMember]
        public Guid? TaskID { get; set; }
        /// <summary>
        /// The workflow activity ID the task is associated with.
        /// </summary>
        [DataMember]
        public Guid? WorkflowActivityID { get; set; }
        /// <summary>
        /// The name of the workflow activity.
        /// </summary>
        [DataMember]
        public string WorkflowActivity { get; set; }
    }
}
