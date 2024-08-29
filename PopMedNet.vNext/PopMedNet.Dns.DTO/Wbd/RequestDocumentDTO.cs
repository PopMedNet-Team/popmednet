using PopMedNet.Objects.ValidationAttributes;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Request Document
    /// </summary>
    [DataContract]
    public class RequestDocumentDTO
    {
        /// <summary>
        /// Gets or sets the ID of document
        /// </summary>
        [DataMember]
        public Guid ID { get; set; }
        /// <summary>
        /// Name
        /// </summary>

        [DataMember]
        [Required, MaxLength(255)]
        public string Name { get; set; }
        /// <summary>
        /// File name
        /// </summary>
        [DataMember]
        [Required]
        public string FileName { get; set; }
        /// <summary>
        /// mime type
        /// </summary>
        [DataMember]
        [Required]
        public string MimeType { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if request document is viewable
        /// </summary>
        [DataMember]
        public bool Viewable { get; set; }
        /// <summary>
        /// Gets or sets the ID of Item
        /// </summary>
        [DataMember]
        public Guid ItemID { get; set; }
    }
}
