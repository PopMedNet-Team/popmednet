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
    /// Document
    /// </summary>
    [DataContract]
    public class DocumentDTO : EntityDtoWithID
    {
        /// <summary>
        /// Document DTO
        /// </summary>
        public DocumentDTO() { }
        /// <summary>
        /// File name, mimetype
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="mimeType"></param>
        /// <param name="viewable"></param>
        /// <param name="kind"></param>
        /// <param name="data"></param>
        public DocumentDTO(string fileName, string mimeType, bool viewable, string kind, byte[] data)
        {
            this.FileName = fileName;
            this.Name = fileName;
            this.MimeType = mimeType;
            this.Kind = kind;
            this.Viewable = viewable;
            this.Data = data;
            if (data != null)
            {
                this.Length = data.LongLength;
            }
        }

        /// <summary>
        /// Document Name
        /// </summary>
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
        /// Determines that the Document is viewable or not
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
        /// Returns Document Data
        /// </summary>
        [DataMember]
        public byte[] Data { get; set; }
        /// <summary>
        /// Returns length of the document
        /// </summary>
        [DataMember]
        public long Length { get; set; }
        /// <summary>
        /// ID of item
        /// </summary>
        [DataMember]
        public Guid ItemID { get; set; }
    }
}
