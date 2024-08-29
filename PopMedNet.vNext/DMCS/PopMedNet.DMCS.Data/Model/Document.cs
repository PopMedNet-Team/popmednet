using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.DMCS.Data.Enums;

namespace PopMedNet.DMCS.Data.Model
{
    [Table("Documents")]
    public class Document : IDocumentMetadata
    {
        public Document()
        {
            CreatedOn = DateTime.UtcNow;
            Version = "1.0.0.0";
            DocumentState = DocumentStates.Local;
        }

        public Guid ID { get; set; }
        public Guid RevisionSetID { get; set; }
        public Guid ItemID { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public long Length { get; set; }        
        public string Version { get; set; }
        public string Kind { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ContentCreatedOn { get; set; }
        public DateTime? ContentModifiedOn { get; set; }
        public Guid? UploadedByID { get; set; }
        public User UploadedBy { get; set; }
        public DocumentStates DocumentState { get; set; }
        public byte[] PmnTimestamp { get; set; }
    }

    public interface IDocumentMetadata
    {
        Guid ID { get; }
        Guid RevisionSetID { get; }
        Guid ItemID { get; }
        string Name { get; }
        string MimeType { get; }
        long Length { get; }
        string Version { get; }
        string Kind { get; }
        DateTime CreatedOn { get; }
        DateTime? ContentCreatedOn { get; }
        DateTime? ContentModifiedOn { get; }
        Guid? UploadedByID { get; }
        byte[] PmnTimestamp { get; }
    }

    public class DocumentMetadataEqualityComparer : IEqualityComparer<IDocumentMetadata>
    {
        public bool Equals(IDocumentMetadata doc1, IDocumentMetadata doc2)
        {
            if (doc1 == null && doc2 == null)
                return true;
            else if (doc1 == null || doc2 == null)
                return false;

            return doc1.ID == doc2.ID &&
                doc1.RevisionSetID == doc2.RevisionSetID &&
                doc1.ItemID == doc2.ItemID &&
                doc1.Name == doc2.Name &&
                doc1.MimeType == doc2.MimeType &&
                doc1.Length == doc2.Length &&
                doc1.Version == doc2.Version &&
                doc1.Kind == doc2.Kind &&
                doc1.CreatedOn == doc2.CreatedOn &&
                doc1.ContentCreatedOn == doc2.ContentCreatedOn &&
                doc1.ContentModifiedOn == doc2.ContentModifiedOn &&
                doc1.UploadedByID == doc2.UploadedByID &&
                ObjectExtensions.ByteEquals(doc1.PmnTimestamp, doc2.PmnTimestamp);
        }

        public int GetHashCode(IDocumentMetadata doc)
        {
            string stringValues = (doc.Name + doc.MimeType + doc.Kind + doc.Version).Ensure();

            var hcode = doc.ID.GetHashCode() ^
                doc.RevisionSetID.GetHashCode() ^
                doc.ItemID.GetHashCode() ^
                stringValues.GetHashCode() ^
                doc.Length.GetHashCode() ^
                doc.CreatedOn.GetHashCode() ^
                (doc.ContentCreatedOn.HasValue ? doc.ContentCreatedOn.Value.GetHashCode() : 0.GetHashCode()) ^
                (doc.ContentModifiedOn.HasValue ? doc.ContentModifiedOn.Value.GetHashCode() : 0.GetHashCode()) ^
                (doc.UploadedByID.HasValue ? doc.UploadedByID.Value.GetHashCode() : 0.GetHashCode()) ^
                doc.PmnTimestamp.GetHashCode();

            return hcode.GetHashCode();
        }
    }
}
