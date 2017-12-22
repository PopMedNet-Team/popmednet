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
    [Table("AclProjectRequestTypes")]
    public class AclProjectRequestType : RequestTypeAcl
    {
        [Key, Column(Order=4)]
        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }

        public virtual RequestType RequestType { get; set; }
    }

    internal class AclProjectRequestTypeSecurityConfiguration : DnsEntitySecurityConfiguration<AclProjectRequestType>
    {
        public override IQueryable<AclProjectRequestType> SecureList(DataContext db, IQueryable<AclProjectRequestType> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.ManageSecurity
                };

            return from q in query join p in  db.Filter(db.Projects, identity, permissions) on q.ProjectID equals p.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclProjectRequestType[] objs)
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

    internal class AclProjectRequestTypeDtoMappingConfiguration : EntityMappingConfiguration<AclProjectRequestType, AclProjectRequestTypeDTO>
    {
        public override System.Linq.Expressions.Expression<Func<AclProjectRequestType, AclProjectRequestTypeDTO>> MapExpression
        {
            get
            {
                return (a) => new AclProjectRequestTypeDTO
                {
                    Overridden = a.Overridden,
                    ProjectID = a.ProjectID,
                    RequestTypeID = a.RequestTypeID,
                    SecurityGroupID = a.SecurityGroupID,
                    SecurityGroup = a.SecurityGroup.Path,
                    Permission = a.Permission
                };
            }
        }
    }
}
