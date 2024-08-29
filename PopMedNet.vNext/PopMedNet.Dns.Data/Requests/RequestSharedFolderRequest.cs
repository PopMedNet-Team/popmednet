using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("RequestSharedFolderRequests")]
    public class RequestSharedFolderRequest
    {
        [Column("RequestSharedFolderID")]
        public Guid FolderID { get; set; }
        public RequestSharedFolder? Folder { get; set; }
        public Guid RequestID { get; set; }
        public Request? Request { get; set; }
    }


    internal class RequestSharedFolderRequestConfiguration : IEntityTypeConfiguration<RequestSharedFolderRequest>
    {
        public void Configure(EntityTypeBuilder<RequestSharedFolderRequest> builder)
        {
            builder.HasKey(e => new { e.FolderID, e.RequestID }).HasName("PK_dbo.RequestSharedFolderRequests");
        }
    }
}
