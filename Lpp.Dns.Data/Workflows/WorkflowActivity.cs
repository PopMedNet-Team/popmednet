using Lpp.Dns.Data.Audit;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.DTO.Events;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Lpp.Utilities.Logging;
using Lpp.Utilities.Objects;
using Lpp.Utilities.Security;
using Lpp.Workflow.Engine.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Net.Mail;

namespace Lpp.Dns.Data
{
    [Table("WorkflowActivities")]
    public class WorkflowActivity : EntityWithID, IDbWorkflowActivity
    {
        [MaxLength(255), Required, Index]
        public string Name { get; set; }
        [MaxLength]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets if the activity is a starting point for a workflow.
        /// </summary>
        public bool Start { get; set; }
        /// <summary>
        /// Gets or sets if the activity is a termination point for a workflow.
        /// </summary>
        public bool End { get; set; }

        public virtual ICollection<WorkflowActivityCompletionMap> SourceMap { get; set; }
        public virtual ICollection<WorkflowActivityCompletionMap> DestinationMap { get; set; }
        public virtual ICollection<WorkflowActivitySecurityGroup> SecurityGroups { get; set; }
        public virtual ICollection<AclProjectRequestTypeWorkflowActivity> ProjectRequestTypeWorkflowActivityAcls { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Response> Responses { get; set; }
    }

    internal class WorkflowActivityConfiguration : EntityTypeConfiguration<WorkflowActivity>
    {
        public WorkflowActivityConfiguration()
        {
            HasMany(t => t.SourceMap).WithRequired(t => t.SourceWorkflowActivity).HasForeignKey(t => t.SourceWorkflowActivityID).WillCascadeOnDelete(true);
            HasMany(t => t.DestinationMap).WithRequired(t => t.DestinationWorkflowActivity).HasForeignKey(t => t.DestinationWorkflowActivityID).WillCascadeOnDelete(false);
            HasMany(t => t.SecurityGroups).WithRequired(t => t.WorkflowActivity).HasForeignKey(t => t.WorkflowActivityID).WillCascadeOnDelete(true);
            HasMany(t => t.ProjectRequestTypeWorkflowActivityAcls).WithRequired(t => t.WorkflowActivity).HasForeignKey(t => t.WorkflowActivityID).WillCascadeOnDelete(true);
            HasMany(t => t.Requests).WithOptional(t => t.WorkflowActivity).HasForeignKey(t => t.WorkFlowActivityID).WillCascadeOnDelete(false);
            HasMany(t => t.Responses).WithOptional(t => t.WorkFlowActivity).HasForeignKey(t => t.WorkFlowActivityID).WillCascadeOnDelete(false);
        }
    }

    internal class WorkflowActivityDtoMappingConfiguration : EntityMappingConfiguration<WorkflowActivity, WorkflowActivityDTO>
    {
        public override System.Linq.Expressions.Expression<Func<WorkflowActivity, WorkflowActivityDTO>> MapExpression
        {
            get
            {
                return (wa) => new WorkflowActivityDTO
                {
                    Description = wa.Description,
                    End = wa.End,
                    ID = wa.ID,
                    Name = wa.Name,
                    Start = wa.Start,
                    Timestamp = wa.Timestamp
                };
            }
        }
    }

}
