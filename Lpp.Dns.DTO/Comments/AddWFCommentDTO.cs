using Lpp.Objects.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// DTO for creating a new workflow activity comment.
    /// </summary>
    [DataContract]
    public class AddWFCommentDTO
    {
        /// <summary>
        /// The ID of the request.
        /// </summary>
        [DataMember, Required]
        public Guid RequestID { get; set; }
        /// <summary>
        /// The ID of the workflow stop the comment is for.
        /// </summary>
        [DataMember]
        public Guid? WorkflowActivityID { get; set; }
        /// <summary>
        /// The comment.
        /// </summary>
        [DataMember, Required]
        public string Comment { get; set; }
    }
}
