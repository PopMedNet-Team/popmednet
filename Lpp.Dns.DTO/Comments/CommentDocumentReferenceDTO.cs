using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// DTO for referencing a Comment Document
    /// </summary>
    [DataContract]
    public class CommentDocumentReferenceDTO
    {
        /// <summary>
        /// Identifier of Comment
        /// </summary>
        [DataMember]
        public Guid CommentID { get; set; }
        /// <summary>
        /// Identifier of Document
        /// </summary>
        [DataMember]
        public Guid? DocumentID { get; set; }
        /// <summary>
        /// Identifies the Revision set
        /// </summary>
        [DataMember]
        public Guid? RevisionSetID { get; set; }
        /// <summary>
        /// The name of the Document
        /// </summary>
        [DataMember]
        public string DocumentName { get; set; }
        /// <summary>
        /// The name of the File 
        /// </summary>
        [DataMember]
        public string FileName { get; set; }
    }
}
