
using Lpp.Utilities.Objects;
using Lpp.Workflow.Engine.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("Workflows")]
    public class Workflow : EntityWithID, IDbWorkflow
    {
        [MaxLength(255), Required, Index]
        public string Name { get; set; }
        [MaxLength]
        public string Description { get; set; }
        public virtual ICollection<RequestType> RequestTypes {get; set;}
        public virtual ICollection<WorkflowRole> Roles { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Response> Responses { get; set; }
    }

    internal class WorkflowConfiguration : EntityTypeConfiguration<Workflow>
    {
        public WorkflowConfiguration()
        {
            HasMany(t => t.RequestTypes).WithOptional(t => t.Workflow).HasForeignKey(t => t.WorkflowID).WillCascadeOnDelete(false);
            HasMany(t => t.Roles).WithRequired(t => t.Workflow).HasForeignKey(t => t.WorkflowID).WillCascadeOnDelete(true);
            HasMany(t => t.Requests).WithOptional(t => t.Workflow).HasForeignKey(t => t.WorkflowID).WillCascadeOnDelete(false);
            HasMany(t => t.Responses).WithOptional(t => t.Workflow).HasForeignKey(t => t.WorkflowID).WillCascadeOnDelete(false);
        }
    }

    internal class WorkflowSecurityConfiguration : DnsEntitySecurityConfiguration<Workflow>
    {

        public override IQueryable<Workflow> SecureList(DataContext db, IQueryable<Workflow> query, Utilities.Security.ApiIdentity identity, params DTO.Security.PermissionDefinition[] permissions)
        {
            return query;
        }

        public override Task<bool> CanInsert(DataContext db, Utilities.Security.ApiIdentity identity, params Workflow[] objs)
        {
            return Task.Run(() => false);
        }

        public override Task<bool> CanDelete(DataContext db, Utilities.Security.ApiIdentity identity, params Guid[] keys)
        {
            return Task.Run(() => false);
        }

        public override Task<bool> CanUpdate(DataContext db, Utilities.Security.ApiIdentity identity, params Guid[] keys)
        {
            return Task.Run(() => false);
        }
    }
}
