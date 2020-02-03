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
    [Table("AclProjects")]
    public class AclProject : Acl
    {
        public AclProject() { }

        [Key, Column(Order = 3)]
        public Guid ProjectID { get; set;}
        public virtual Project Project { get; set; }
    }

    internal class AclProjectSecurityConfiguration : DnsEntitySecurityConfiguration<AclProject>
    {
        public override IQueryable<AclProject> SecureList(DataContext db, IQueryable<AclProject> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.ManageSecurity
                };

            return from q in query join p in db.Filter(db.Projects, identity, permissions) on q.ProjectID equals p.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclProject[] objs)
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

    internal class AclProjectDTOMappingConfiguration : EntityMappingConfiguration<AclProject, AclProjectDTO>
    {
        public override System.Linq.Expressions.Expression<Func<AclProject, AclProjectDTO>> MapExpression
        {
            get
            {
                return (q) => new AclProjectDTO
                {
                    Allowed = q.Allowed,
                    Overridden = q.Overridden,
                    PermissionID = q.PermissionID,
                    Permission = q.Permission.Name,
                    ProjectID = q.ProjectID,
                    SecurityGroupID = q.SecurityGroupID,
                    SecurityGroup = q.SecurityGroup.Path
                };
            }
        }
    }
}
