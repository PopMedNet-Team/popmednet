using Lpp.Utilities.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("WorkflowRoles")]
    public class WorkflowRole : EntityWithID
    {

        public WorkflowRole() : base()
        {
            IsRequestCreator = false;
        }

        public Guid WorkflowID { get; set; }
        public virtual Workflow Workflow { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required, DefaultValue(false)]
        public bool IsRequestCreator { get; set; }

        public virtual ICollection<RequestUser> RequestUsers { get; set; }
    }

    internal class WorkflowRoleConfiguration : EntityTypeConfiguration<WorkflowRole>
    {
        public WorkflowRoleConfiguration()
        {
            HasMany(t => t.RequestUsers).WithRequired(t => t.WorkflowRole).HasForeignKey(t => t.WorkflowRoleID).WillCascadeOnDelete(true);
        }
    }
}
