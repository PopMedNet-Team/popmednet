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
    [Table("AclRequestTypes")]
    public class AclRequestType : Acl
    {
        public AclRequestType() { }

        [Column(Order = 3), Key]
        public Guid RequestTypeID { get; set; }

        public virtual RequestType RequestType { get; set; }
    }

    internal class AclRequestTypeSecurityConfiguration : DnsEntitySecurityConfiguration<AclRequestType>
    {
        public override IQueryable<AclRequestType> SecureList(DataContext db, IQueryable<AclRequestType> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.RequestTypes.ManageSecurity, PermissionIdentifiers.RequestTypes.View
                };

            return from q in query join r in db.Filter(db.RequestTypes, identity, permissions) on q.RequestTypeID equals r.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclRequestType[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.RequestTypes.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.RequestTypes.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.RequestTypes.ManageSecurity);
        }
    }

    internal class AclRequestTypeDTOMappingConfiguration : EntityMappingConfiguration<AclRequestType, Lpp.Dns.DTO.AclRequestTypeDTO>
    {
        public override System.Linq.Expressions.Expression<Func<AclRequestType, DTO.AclRequestTypeDTO>> MapExpression
        {
            get
            {
                return (t) => new Lpp.Dns.DTO.AclRequestTypeDTO
                {
                    Allowed = t.Allowed,
                    Overridden = t.Overridden,
                    Permission = t.Permission.Name,
                    PermissionID = t.PermissionID,
                    SecurityGroup = t.SecurityGroup.Path,
                    SecurityGroupID = t.SecurityGroupID,
                    RequestTypeID = t.RequestTypeID
                };
            }
        }
    }

}
