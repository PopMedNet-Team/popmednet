using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Objects;

namespace PopMedNet.Dns.Data
{
    [Table("RequestSharedFolders")]
    public class RequestSharedFolder : EntityWithID, IEntityWithName
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }

        public virtual ICollection<RequestSharedFolderRequest> Requests { get; set; } = new HashSet<RequestSharedFolderRequest>();

        public virtual ICollection<AclRequestSharedFolder> RequestSharedFolderAcls { get; set; } = new HashSet<AclRequestSharedFolder>();
    }

    internal class RequestSharedFolderConfiguration : IEntityTypeConfiguration<RequestSharedFolder>
    {
        public void Configure(EntityTypeBuilder<RequestSharedFolder> builder)
        {
            builder.HasMany(t => t.Requests)
                .WithOne(t => t.Folder)
                .IsRequired(true)
                .HasForeignKey(t => t.FolderID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.RequestSharedFolderAcls)
                .WithOne(t => t.RequestSharedFolder)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestSharedFolderID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
