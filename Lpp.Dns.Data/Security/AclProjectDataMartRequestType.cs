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
    [Table("AclProjectDataMartRequestTypes")]
    public class AclProjectDataMartRequestType : RequestTypeAcl
    {
        [Key, Column(Order = 4)]
        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }
        [Key, Column(Order = 5)]
        public Guid DataMartID { get; set; }
        public virtual DataMart DataMart { get; set; }

        public virtual RequestType RequestType { get; set; }
    }

    internal class AclProjectDataMartRequestTypeSecurityConfiguration : DnsEntitySecurityConfiguration<AclProjectDataMartRequestType>
    {
        public override IQueryable<AclProjectDataMartRequestType> SecureList(DataContext db, IQueryable<AclProjectDataMartRequestType> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.ManageSecurity
                };

            return from q in query join p in db.Filter(db.Projects, identity, permissions) on q.ProjectID equals p.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclProjectDataMartRequestType[] objs)
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

    internal class AclProjectDataMartRequestTypeDTOMappingConfiguration : EntityMappingConfiguration<AclProjectDataMartRequestType, AclProjectDataMartRequestTypeDTO>
    {
        public override System.Linq.Expressions.Expression<Func<AclProjectDataMartRequestType, AclProjectDataMartRequestTypeDTO>> MapExpression
        {
            get
            {
                return (d) => new AclProjectDataMartRequestTypeDTO
                {
                    DataMartID = d.DataMartID,
                    Overridden = d.Overridden,
                    Permission = d.Permission,
                    ProjectID = d.ProjectID,
                    RequestTypeID = d.RequestTypeID,
                    SecurityGroup = d.SecurityGroup.Path,
                    SecurityGroupID = d.SecurityGroupID
                };
            }
        }
    }
}
