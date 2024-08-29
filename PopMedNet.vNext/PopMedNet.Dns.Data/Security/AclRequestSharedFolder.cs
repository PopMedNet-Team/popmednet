using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("AclRequestSharedFolders")]
    public class AclRequestSharedFolder : Acl
    {
        public Guid RequestSharedFolderID { get; set; }
        public virtual RequestSharedFolder? RequestSharedFolder { get; set; }
    }
    internal class AclRequestSharedFolderConfiguration : IEntityTypeConfiguration<AclRequestSharedFolder>
    {
        public void Configure(EntityTypeBuilder<AclRequestSharedFolder> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.PermissionID, e.RequestSharedFolderID }).HasName("PK_dbo.AclRequestSharedFolders");
        }
    }
}
