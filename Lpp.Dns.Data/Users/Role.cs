using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;

namespace Lpp.Dns.Data
{
    [Table("Roles")]
    public class Role : EntityWithID
    {
        public Role()
        {
            this.Description = string.Empty;
            this.Name = string.Empty;
            this.Deleted = false;
        }

        [MaxLength(200), Required]
        public string Name { get; set; }

        [Column("isDeleted")]
        public bool Deleted { get; set; }

        [Required(AllowEmptyStrings=true)]
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }

    internal class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            HasMany(t => t.Users).WithOptional(t => t.Role).HasForeignKey(t => t.RoleID).WillCascadeOnDelete(false);
        }
    }
}
