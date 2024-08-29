using PopMedNet.DMCS.Data.Model;
using System;

namespace PopMedNet.DMCS.PMNApi.PMNDto
{
    public class DMCSDocument : Data.Model.IDocumentMetadata
    {
        public Guid ID { get; set; }
        public Guid RevisionSetID { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public long Length { get; set; }
        public byte[] Timestamp { get; set; }
        public string Version { get; set; }
        public string Kind { get; set;  }
        public Guid ItemID { get; set; }
        public DateTime? ContentCreatedOn { get; set; }
        public DateTime? ContentModifiedOn { get; set; }
        public Guid? UploadedByID { get; set; }
        public string UploadedByUserName { get; set; }
        public string UploadedByEmail { get; set; }
        public DateTime CreatedOn { get; set; }
        byte[] IDocumentMetadata.PmnTimestamp => Timestamp;
    }
}
