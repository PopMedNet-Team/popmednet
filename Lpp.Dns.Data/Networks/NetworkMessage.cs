using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;
using Lpp.Utilities.Security;
using Lpp.Utilities;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Data
{
    [Table("NetworkMessages")]
    public class NetworkMessage : EntityWithID
    {
        public NetworkMessage()
        {
            CreatedOn = DateTime.UtcNow;
            Users = new HashSet<NetworkMessageUser>();
        }

        [Required(AllowEmptyStrings=true)]
        public string Subject { get; set; }
        [Required]
        public string MessageText { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual ICollection<NetworkMessageUser> Users { get; set; }
    }

    internal class NetworkMessageConfiguration : EntityTypeConfiguration<NetworkMessage>
    {
        public NetworkMessageConfiguration()
        {
            HasMany(t => t.Users)
                .WithRequired(t => t.NetworkMessage)
                .HasForeignKey(t => t.NetworkMessageID)
                .WillCascadeOnDelete(true);
        }
    }

    internal class NetworkMessageSecurityConfiguration : DnsEntitySecurityConfiguration<NetworkMessage>
    {
        public override IQueryable<NetworkMessage> SecureList(DataContext db, IQueryable<NetworkMessage> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            return query;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params NetworkMessage[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Portal.CreateNetworkMessages);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Portal.CreateNetworkMessages); //Should be its own right
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Portal.CreateNetworkMessages); //Should be its own right
        }

    }

    internal class NetworkMessageDtoMappingConfiguration : EntityMappingConfiguration<NetworkMessage, NetworkMessageDTO>
    {
        public override System.Linq.Expressions.Expression<Func<NetworkMessage, NetworkMessageDTO>> MapExpression
        {
            get
            {
                return (m) => new NetworkMessageDTO
                {
                    CreatedOn = m.CreatedOn,
                    ID = m.ID,
                    MessageText = m.MessageText,
                    Subject = m.Subject,
                    Timestamp = m.Timestamp
                };
            }
        }
    }
}
