using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("AclRequests")]
    public class AclRequest : Acl
    {
        public AclRequest() { }

        public Guid RequestID { get; set; }
        public virtual Request? Request { get; set; }

    }
    internal class AclRequestConfiguration : IEntityTypeConfiguration<AclRequest>
    {
        public void Configure(EntityTypeBuilder<AclRequest> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.PermissionID, e.RequestID }).HasName("PK_dbo.AclRequests");
        }
    }
}
