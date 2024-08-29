using PopMedNet.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// A Comment 
    /// </summary>
    [DataContract]
    public class CommentDTO : EntityDtoWithID
    {
        /// <summary>
        /// The Comment content
        /// </summary>
        [DataMember]
        public string Comment { get; set; }
        /// <summary>
        /// ID of item
        /// </summary>
        [DataMember]
        public Guid ItemID { get; set; }
        /// <summary>
        /// Title of Item
        /// </summary>
        [DataMember]
        public string ItemTitle { get; set; }
        /// <summary>
        /// The date the comment was Created On
        /// </summary>
        [DataMember]
        public DateTimeOffset CreatedOn { get; set; }
        /// <summary>
        /// The ID of the user that created the comment
        /// </summary>
        [DataMember]
        public Guid CreatedByID { get; set; }
        /// <summary>
        /// The username of the person that created the comment
        /// </summary>
        [DataMember]
        public string CreatedBy { get; set; }
    }
}
