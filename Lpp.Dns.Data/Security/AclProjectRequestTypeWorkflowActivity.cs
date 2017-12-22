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
    [Table("AclProjectRequestTypeWorkflowActivities")]
    public class AclProjectRequestTypeWorkflowActivity : Acl
    {
        [Key, Column(Order = 3)]
        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }
        [Key, Column(Order = 4)]
        public Guid RequestTypeID { get; set; }
        public virtual RequestType RequestType { get; set; }
        [Key, Column(Order = 5)]
        public Guid WorkflowActivityID { get; set; }
        public virtual WorkflowActivity WorkflowActivity { get; set; }
    }

    internal class AclProjectRequestTypeWorkflowActivitySecurityConfiguration : DnsEntitySecurityConfiguration<AclProjectRequestTypeWorkflowActivity>
    {
        public override IQueryable<AclProjectRequestTypeWorkflowActivity> SecureList(DataContext db, IQueryable<AclProjectRequestTypeWorkflowActivity> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            return from q in query join p in db.Filter<Project>(db.Projects, identity, PermissionIdentifiers.Project.ManageSecurity) on q.ProjectID equals p.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclProjectRequestTypeWorkflowActivity[] objs)
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

    internal class AclProjectRequestTypeWorkflowActivityDtoMappingConfiguration : EntityMappingConfiguration<AclProjectRequestTypeWorkflowActivity, AclProjectRequestTypeWorkflowActivityDTO>
    {
        public override System.Linq.Expressions.Expression<Func<AclProjectRequestTypeWorkflowActivity, AclProjectRequestTypeWorkflowActivityDTO>> MapExpression
        {
            get
            {
                return (a) => new AclProjectRequestTypeWorkflowActivityDTO
                {
                    Allowed = a.Allowed,
                    Overridden = a.Overridden,
                    Permission = a.Permission.Name,
                    PermissionID = a.PermissionID,
                    Project = a.Project.Name,
                    ProjectID = a.ProjectID,
                    RequestType = a.RequestType.Name,
                    RequestTypeID = a.RequestTypeID,
                    SecurityGroup = a.SecurityGroup.Path,
                    SecurityGroupID = a.SecurityGroupID,
                    WorkflowActivity = a.WorkflowActivity.Name,
                    WorkflowActivityID = a.WorkflowActivityID
                };
            }
        }
    }
}
