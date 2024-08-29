using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.Data.Documents;
using System.Data.SqlClient;

namespace PopMedNet.Dns.Data
{
    [Table("Documents")]
    public class Document : EntityWithID
    {
        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        [MaxLength(255), Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the filename of the document.
        /// </summary>
        [MaxLength(255), Required]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets if the document is viewable (has a visualizer).
        /// </summary>
        [Column("isViewable")]
        public bool Viewable { get; set; }

        /// <summary>
        /// Gets or sets the mime type of the document.
        /// </summary>
        [Required, MaxLength(100)]
        public string MimeType { get; set; } = "application/octet-stream";

        /// <summary>
        /// Gets or sets the document kind.
        /// </summary>
        [MaxLength(50)]
        public string? Kind { get; set; }

        /// <summary>
        /// Gets or sets the created on date.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last time the document content was modified.
        /// </summary>
        public DateTime? ContentModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the first time the document content was created.
        /// </summary>
        public DateTime? ContentCreatedOn { get; set; }

        /// <summary>
        /// Gets or set the length of the document in bytes.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// Gets or set the ID of the item the document is associated with, ie. Request, Response, Task, etc...
        /// </summary>
        public Guid ItemID { get; set; }

        /// <summary>
        /// Gets or set the description of the document.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the ID of the document the current document is a revision of.
        /// </summary>
        public Guid? ParentDocumentID { get; set; }
        public virtual Document? ParentDocument { get; set; }

        /// <summary>
        /// Gets or set the ID of the user who uploaded the document.
        /// </summary>
        public Guid? UploadedByID { get; set; }
        public virtual User? UploadedBy { get; set; }

        /// <summary>
        /// Gets or sets an identifier that groups a set of revisions of a specific document.
        /// </summary>
        public Guid? RevisionSetID { get; set; }
        /// <summary>
        /// Gets or set a description of the revision.
        /// </summary>
        public string? RevisionDescription { get; set; }
        /// <summary>
        /// Gets or sets the major version number. Version format: {major}.{minor}.{build}.{revision}
        /// </summary>
        public int MajorVersion { get; set; } = 1;
        /// <summary>
        /// Gets or sets the minor version number. Version format: {major}.{minor}.{build}.{revision}
        /// </summary>
        public int MinorVersion { get; set; } = 0;
        /// <summary>
        /// Gets or sets the build version number. Version format: {major}.{minor}.{build}.{revision}
        /// </summary>
        public int BuildVersion { get; set; } = 0;
        /// <summary>
        /// Gets or sets the revision version number. Version format: {major}.{minor}.{build}.{revision}
        /// </summary>
        public int RevisionVersion { get; set; } = 0;

        public virtual ICollection<Document> Documents { get; set; } = new HashSet<Document>();

        public virtual ICollection<Audit.DocumentChangeLog> DocumentChangeLogs { get; set; } = new HashSet<Audit.DocumentChangeLog>();

        /// <summary>
        /// Gets the Data of the document
        /// </summary>
        /// <param name="db">The data context that the document is attached to</param>
        /// <returns></returns>
        public byte[] GetData(DataContext db)
        {
            using (var stream = new DocumentStream(db, this.ID))
            {
                var buffer = new byte[stream.Length];
                if (this.Length != buffer.Length)
                    this.Length = buffer.Length;

                stream.Read(buffer, 0, buffer.Length);

                return buffer;
            }
        }

        /// <summary>
        /// Returns a stream to read and write to and from
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public DocumentStream GetStream(DataContext db)
        {
            return new DocumentStream(db, this.ID);
        }

        /// <summary>
        /// Returns the string representation of the document
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public string ReadStreamAsString(DataContext db)
        {
            return System.Text.UTF8Encoding.UTF8.GetString(this.GetData(db));
        }

        /// <summary>
        /// Sets the data for the document
        /// </summary>
        /// <param name="db">The Data context that the document is attached to</param>
        /// <param name="data">The Data to write</param>
        public void SetData(DataContext db, byte[] data)
        {
            using (var stream = new DocumentStream(db, this.ID))
            {
                //this.Length = data.Length;
                stream.Write(data, 0, data.Length);
            }

            db.Database.ExecuteSqlRaw("UPDATE Documents SET ContentModifiedOn = GETUTCDATE(), ContentCreatedOn = CASE WHEN ContentCreatedOn IS NULL THEN GETUTCDATE() ELSE ContentCreatedOn END WHERE ID = @ID", new SqlParameter("ID", ID.ToString("D")));
        }

        public void CopyData(DataContext db, Guid sourceDocumentID)
        {
            DateTime contentCreated = DateTime.UtcNow;
            using (var sourceStream = new DocumentStream(db, sourceDocumentID))
            {
                using (var destinationStream = new DocumentStream(db, this.ID))
                {
                    sourceStream.CopyTo(destinationStream);
                    this.Length = sourceStream.Length;
                    destinationStream.Flush();
                }
                sourceStream.Flush();
            }

            db.Database.ExecuteSqlRaw("UPDATE Documents SET ContentModifiedOn = GETUTCDATE(), ContentCreatedOn = @ContentCreated WHERE ID = @ID", new SqlParameter("ContentCreated", contentCreated), new SqlParameter("ID", ID.ToString("D")));
        }

    }

    //public class FileStreamRowData
    //{
    //    public FileStreamRowData() { }

    //    public string Path { get; set; }
    //    public byte[] Transaction { get; set; }
    //}

    internal class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasIndex(d => d.Name).IsClustered(false).IsUnique(false);
            builder.HasIndex(d => d.FileName).IsClustered(false).IsUnique(false);
            builder.HasIndex(d => d.ItemID).IsClustered(false).IsUnique(false);

            builder.Property(p => p.ID).ValueGeneratedNever();

            builder.HasMany(d => d.Documents)
                .WithOne(d => d.ParentDocument)
                .IsRequired(false)
                .HasForeignKey(d => d.ParentDocumentID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(d => d.DocumentChangeLogs)
                .WithOne(d => d.Document)
                .IsRequired(true)
                .HasForeignKey(d => d.DocumentID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }



}
