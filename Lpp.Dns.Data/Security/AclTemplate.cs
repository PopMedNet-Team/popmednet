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
    [Table("AclTemplates")]
    public class AclTemplate : Acl
    {
        [Key, Column(Order = 3)]
        public Guid TemplateID { get; set; }
        public virtual Template Template { get; set; }
    }

    internal class AclTemplateSecurityConfiguration : DnsEntitySecurityConfiguration<AclTemplate>
    {
        public override IQueryable<AclTemplate> SecureList(DataContext db, IQueryable<AclTemplate> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Templates.ManageSecurity
                };

            return from q in query join r in db.Filter(db.Templates, identity, permissions) on q.TemplateID equals r.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclTemplate[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Templates.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Templates.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Templates.ManageSecurity);
        }
    }

    internal class AclTemplateDTOMappingConfiguration : EntityMappingConfiguration<AclTemplate, Lpp.Dns.DTO.AclTemplateDTO>
    {
        public override System.Linq.Expressions.Expression<Func<AclTemplate, DTO.AclTemplateDTO>> MapExpression
        {
            get
            {
                return (t) => new Lpp.Dns.DTO.AclTemplateDTO
                {
                    Allowed = t.Allowed,
                    Overridden = t.Overridden,
                    Permission = t.Permission.Name,
                    PermissionID = t.PermissionID, 
                    SecurityGroup = t.SecurityGroup.Path,
                    SecurityGroupID = t.SecurityGroupID,
                    TemplateID = t.TemplateID
                };
            }
        }
    }
}
