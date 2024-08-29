using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Utilities.Objects;
using PopMedNet.Workflow.Engine.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopMedNet.Dns.Data
{
    [Table("Workflows")]
    public class Workflow : EntityWithID, IDbWorkflow
    {
        [MaxLength(255), Required]
        public string Name { get; set; }
        [MaxLength]
        public string Description { get; set; }
        public virtual ICollection<RequestType> RequestTypes { get; set; } = new HashSet<RequestType>();
        public virtual ICollection<WorkflowRole> Roles { get; set; } = new HashSet<WorkflowRole>();
        public virtual ICollection<Request> Requests { get; set; } = new HashSet<Request>();
        public virtual ICollection<Response> Responses { get; set; } = new HashSet<Response>();
    }

    internal class WorkflowConfiguration : IEntityTypeConfiguration<Workflow>
    {
        public void Configure(EntityTypeBuilder<Workflow> builder)
        {
            builder.HasIndex(i => i.Name).IsClustered(false).IsUnique(false);

            builder.HasMany(t => t.RequestTypes)
                .WithOne(t => t.Workflow)
                .IsRequired(false)
                .HasForeignKey(t => t.WorkflowID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(t => t.Roles)
                .WithOne(t => t.Workflow)
                .IsRequired(true)
                .HasForeignKey(t => t.WorkflowID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Requests)
                .WithOne(t => t.Workflow)
                .IsRequired(false)
                .HasForeignKey(t => t.WorkflowID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(t => t.Responses)
                .WithOne(t => t.Workflow)
                .IsRequired(false)
                .HasForeignKey(t => t.WorkflowID)
                .OnDelete(DeleteBehavior.ClientSetNull);
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

    internal class WorkflowMappingProfile : AutoMapper.Profile
    {
        public WorkflowMappingProfile()
        {
            CreateMap<Workflow, DTO.WorkflowDTO>();
        }
    }
}
