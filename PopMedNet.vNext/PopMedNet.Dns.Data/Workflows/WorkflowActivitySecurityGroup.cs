using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("WorkflowActivitySecurityGroups")]
    public class WorkflowActivitySecurityGroup : Entity
    {
        public Guid WorkflowActivityID { get; set; }
        public virtual WorkflowActivity? WorkflowActivity { get; set; }
        public Guid SecurityGroupID { get; set; }
        public virtual SecurityGroup? SecurityGroup { get; set; }
    }
    internal class WorkflowActivitySecurityGroupConfiguration : IEntityTypeConfiguration<WorkflowActivitySecurityGroup>
    {
        public void Configure(EntityTypeBuilder<WorkflowActivitySecurityGroup> builder)
        {
            builder.HasKey(e => new { e.WorkflowActivityID, e.SecurityGroupID }).HasName("PK_dbo.WorkflowActivitySecurityGroups");
        }
    }
}
