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
    [Table("AclProjectDataMarts")]
    public class AclProjectDataMart : Acl
    {
        [Key, Column(Order = 3)]
        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }

        [Key, Column(Order = 4)]
        public Guid DataMartID { get; set; }
        public virtual DataMart DataMart { get; set; }
    }

    internal class AclProjectDataMartSecurityConfiguration : DnsEntitySecurityConfiguration<AclProjectDataMart>
    {
        public override IQueryable<AclProjectDataMart> SecureList(DataContext db, IQueryable<AclProjectDataMart> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.ManageSecurity
                };

            return from q in query join p in db.Filter(db.Projects, identity, permissions) on q.ProjectID equals p.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclProjectDataMart[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Project.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Project.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Project.ManageSecurity);
        }
    }

    internal class AclProjectDataMartDTOMappingConfiguration : EntityMappingConfiguration<AclProjectDataMart, AclProjectDataMartDTO>
    {

        public override System.Linq.Expressions.Expression<Func<AclProjectDataMart, AclProjectDataMartDTO>> MapExpression
        {
            get
            {
                return (a) => new AclProjectDataMartDTO
                {
                    Allowed = a.Allowed,
                    DataMartID = a.DataMartID,
                    Overridden = a.Overridden,
                    PermissionID = a.PermissionID,
                    Permission = a.Permission != null ? a.Permission.Name : null,
                    ProjectID = a.ProjectID,
                    SecurityGroup = a.SecurityGroup != null ? a.SecurityGroup.Path : null,
                    SecurityGroupID = a.SecurityGroupID
                };
            }
        }

    }
}
