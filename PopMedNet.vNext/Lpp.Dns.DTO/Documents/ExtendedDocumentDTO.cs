using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Extended Document
    /// </summary>
    [DataContract]
    public class ExtendedDocumentDTO : EntityDtoWithID
    {
        
        /** Default document properties **/
        
        [DataMember]
        [MaxLength(255), Required]
        public string Name { get; set; }
        /// <summary>
        /// File Name
        /// </summary>
        [DataMember]
        [MaxLength(255), Required]
        public string FileName { get; set; }
        /// <summary>
        /// Determines that Document is viewable or not
        /// </summary>
        [DataMember]
        public bool Viewable { get; set; }
        /// <summary>
        /// Mime Type
        /// </summary>
        [DataMember]
        [Required, MaxLength(100)]
        public string MimeType { get; set; }
        /// <summary>
        /// Kind
        /// </summary>
        [DataMember]
        [MaxLength(50)]
        public string Kind { get; set; }
        /// <summary>
        /// Returns Length of Document
        /// </summary>
        [DataMember]
        public long Length { get; set; }
        /// <summary>
        /// ID of item
        /// </summary>
        [DataMember]
        public Guid ItemID { get; set; }
        /** Extended Properties **/
        [DataMember]
        public DateTimeOffset CreatedOn { get; set; }
        /// <summary>
        /// The time the content of the document was last modified.
        /// </summary>
        [DataMember]
        public DateTimeOffset? ContentModifiedOn { get; set; }
        /// <summary>
        /// The time the content of the document was first persisted to the database.
        /// </summary>
        [DataMember]
        public DateTimeOffset? ContentCreatedOn { get; set; }
        /// <summary>
        /// Item Title
        /// </summary>
        [DataMember]
        public string ItemTitle { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// ID of Parent Document
        /// </summary>
        [DataMember]
        public Guid? ParentDocumentID { get; set; }
        /// <summary>
        /// The ID of the user that uploaded the document
        /// </summary>
        [DataMember]
        public Guid? UploadedByID { get; set; }
        /// <summary>
        /// Returns who uploaded the document
        /// </summary>
        [DataMember]
        public string UploadedBy { get; set; }
        /// <summary>
        /// ID of Revision set
        /// </summary>
        [DataMember]
        public Guid? RevisionSetID { get; set; }
        /// <summary>
        /// Revision Description
        /// </summary>
        [DataMember]
        public string RevisionDescription { get; set; }
        /// <summary>
        /// Returns Major version
        /// </summary>
        [DataMember]
        public int MajorVersion { get; set; }
        /// <summary>
        /// Returns Minor version
        /// </summary>
        [DataMember]
        public int MinorVersion { get; set; }
        /// <summary>
        /// Returns the build version
        /// </summary>
        [DataMember]
        public int BuildVersion { get; set; }
        /// <summary>
        /// Returns Revision version
        /// </summary>
        [DataMember]
        public int RevisionVersion { get; set; }
        /// <summary>
        /// Gets or sets the associated task item type for the document if it exists.
        /// </summary>
        [DataMember]
        public DTO.Enums.TaskItemTypes? TaskItemType { get; set; }
        /// <summary>
        /// Gets or Sets if the Document was Input or Output from the DMC
        /// </summary>
        [DataMember]
        public DTO.Enums.RequestDocumentType? DocumentType { get; set; }

    }
}
