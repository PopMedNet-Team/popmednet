using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("SecurityGroupUsers")]
    public class SecurityGroupUser : Entity
    {
        public SecurityGroupUser() { }

        public Guid SecurityGroupID { get; set; }
        public SecurityGroup? SecurityGroup { get; set; }
        public Guid UserID { get; set; }
        public User? User { get; set; }
        public bool Overridden { get; set; }
    }
    internal class SecurityGroupUserConfiguration : IEntityTypeConfiguration<SecurityGroupUser>
    {
        public void Configure(EntityTypeBuilder<SecurityGroupUser> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.UserID }).HasName("PK_dbo.SecurityGroupUsers");
        }
    }
}
