using PopMedNet.Utilities;
using PopMedNet.Utilities.Objects;
using PopMedNet.Utilities.Security;
using PopMedNet.Dns.DTO;
using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopMedNet.Dns.Data
{
    [Table("Roles")]
    public class Role : EntityWithID
    {
        public Role()
        {
            this.Description = string.Empty;
            this.Name = string.Empty;
            this.Deleted = false;
            Users = new List<User>();
        }

        [MaxLength(200), Required]
        public string Name { get; set; }

        [Column("isDeleted")]
        public bool Deleted { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }

    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasMany(r => r.Users).WithOne(u => u.Role).IsRequired(false).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
