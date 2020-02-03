using Lpp.Utilities.Objects;
using Lpp.Workflow.Engine.Database;
using Lpp.Workflow.Engine.Interfaces;
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
    [Table("WorkflowActivityResults")]
    public class WorkflowActivityResult : EntityWithID, IDbWorkflowActivityResult
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }
        [MaxLength]
        public string Description { get; set; }
        [MaxLength]
        public string Uri { get; set; }

        public virtual ICollection<WorkflowActivityCompletionMap> Maps { get; set; }       
    }

    internal class WorkflowActivityResultConfiguration : EntityTypeConfiguration<WorkflowActivityResult>
    {
        public WorkflowActivityResultConfiguration()
        {
            HasMany(t => t.Maps).WithRequired(t => t.WorkflowActivityResult).HasForeignKey(t => t.WorkflowActivityResultID).WillCascadeOnDelete(true);
        }
    }
}
