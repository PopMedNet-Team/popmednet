using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("AclOrganizationUsers")]
    public class AclOrganizationUser : Acl
    {
        public Guid OrganizationID { get; set; }
        public virtual Organization? Organization { get; set; }
        public Guid UserID { get; set; }
        public virtual User? User { get; set; }
    }
    internal class AclOrganizationUserConfiguration : IEntityTypeConfiguration<AclOrganizationUser>
    {
        public void Configure(EntityTypeBuilder<AclOrganizationUser> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.PermissionID, e.OrganizationID, e.UserID }).HasName("PK_dbo.AclOrganizationUsers");
        }
    }
}
