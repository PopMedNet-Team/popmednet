using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Lpp.Utilities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("AclDataMarts")]
    public class AclDataMart : Acl
    {
        [Key, Column(Order = 3)]
        public Guid DataMartID { get; set; }
        public virtual DataMart DataMart { get; set; }    
    }

    internal class AclDataMartSecurityConfiguration : DnsEntitySecurityConfiguration<AclDataMart>
    {

        public override IQueryable<AclDataMart> SecureList(DataContext db, IQueryable<AclDataMart> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] { PermissionIdentifiers.DataMart.ManageSecurity };

            return from q in query join dm in db.Filter(db.DataMarts, identity, permissions) on q.DataMartID equals dm.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclDataMart[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.DataMart.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.DataMart.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.DataMart.ManageSecurity);
        }
    }

    internal class AclDataMartDTOMappingConfiguration : EntityMappingConfiguration<AclDataMart, AclDataMartDTO>
    {
        public override System.Linq.Expressions.Expression<Func<AclDataMart, AclDataMartDTO>> MapExpression
        {
            get
            {
                return (dm) => new AclDataMartDTO
                {
                    Allowed = dm.Allowed,
                    DataMartID = dm.DataMartID,
                    Overridden = dm.Overridden,
                    Permission = dm.Permission.Name,
                    PermissionID = dm.PermissionID,
                    SecurityGroup = dm.SecurityGroup.Path,
                    SecurityGroupID = dm.SecurityGroupID
                };
            }
        }
    }
}
