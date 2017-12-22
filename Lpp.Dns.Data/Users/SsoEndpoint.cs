using Lpp.Dns.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;
using Lpp.Utilities.Security;

namespace Lpp.Dns.Data
{
    [Table("SsoEndpoints")]
    public class SsoEndpoint : EntityWithID
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required, MaxLength(255)]
        public string PostUrl { get; set; }
        
        [MaxLength(150)]
        public string oAuthKey { get; set; }

        [MaxLength(150)]
        public string oAuthHash { get; set; }

        public bool RequirePassword { get; set; }

        public Guid Group { get; set; }

        public int DisplayIndex { get; set; }

        public bool Enabled { get; set; }
    }

    internal class SsoEndpointSecurityConfiguration : DnsEntitySecurityConfiguration<SsoEndpoint>
    {

        public override IQueryable<SsoEndpoint> SecureList(DataContext db, IQueryable<SsoEndpoint> query, ApiIdentity identity, params DTO.Security.PermissionDefinition[] permissions)
        {
            return query; //Filter this at some point
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params SsoEndpoint[] objs)
        {
            return Task.Run(() => false);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return Task.Run(() => false);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return Task.Run(() => false);
        }
    }
}
