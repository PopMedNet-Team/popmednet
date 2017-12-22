using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;
using Lpp.Utilities.Security;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Data
{
    [Table("Networks")]
    public class Network : EntityWithID
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(450)]
        public string Url { get; set; }

        public virtual ICollection<WorkplanType> WorkplanTypes { get; set; }
        public virtual ICollection<RequesterCenter> RequesterCenters { get; set; }
    }

    internal class NetworkConfiguration : EntityTypeConfiguration<Network>
    {
        public NetworkConfiguration()
        {
            HasMany(t => t.WorkplanTypes).WithRequired(t => t.Network).HasForeignKey(t => t.NetworkID).WillCascadeOnDelete(true);
            HasMany(t => t.RequesterCenters).WithRequired(t => t.Network).HasForeignKey(t => t.NetworkID).WillCascadeOnDelete(true);
        }
    }

    internal class NetworkSecurityConfiguration : DnsEntitySecurityConfiguration<Network>
    {
        public override IQueryable<Network> SecureList(DataContext db, IQueryable<Network> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Organization.View
                };

            return db.Filter(query, identity, permissions);
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params Network[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Portal.CreateOrganizations);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Organization.Delete);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Organization.Edit);
        }
    }
}
