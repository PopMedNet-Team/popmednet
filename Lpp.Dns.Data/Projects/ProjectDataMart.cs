using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects;
using Lpp.Utilities.Security;
using Lpp.Dns.DTO;
using Lpp.Utilities;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Data
{
    [Table("ProjectDataMarts")]
    public class ProjectDataMart : Entity
    {
        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None), Column(Order=0)]
        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }

        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None), Column(Order = 1)]
        public Guid DataMartID { get; set; }
    
        public virtual DataMart DataMart { get; set; }
    }

    internal class ProjectDataMartSecurityConfiguration : DnsEntitySecurityConfiguration<ProjectDataMart>
    {
        public override IQueryable<ProjectDataMart> SecureList(DataContext db, IQueryable<ProjectDataMart> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.View
                };

            return from pdm in query join p in db.Filter(db.Projects, identity, permissions) on pdm.ProjectID equals p.ID select pdm;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params ProjectDataMart[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.ProjectID).ToArray(), PermissionIdentifiers.Project.Edit);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project DataMart does not have direct permissions for delete, check it's parent project");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project DataMart does not have direct permissions for update, check it's parent project");
        }
    }

    internal class ProjectDataMartDtoMappingConfiguration : EntityMappingConfiguration<ProjectDataMart, ProjectDataMartDTO>
    {
        public override System.Linq.Expressions.Expression<Func<ProjectDataMart, ProjectDataMartDTO>> MapExpression
        {
            get
            {
                return (dm) => new ProjectDataMartDTO
                {
                    DataMartID = dm.DataMartID,
                    Organization = dm.DataMart.Organization.Name,
                    ProjectID = dm.ProjectID,
                    Project = dm.Project.Name,
                    ProjectAcronym = dm.Project.Acronym,
                    DataMart = dm.DataMart.Name
                };
            }
        }
    }
}
