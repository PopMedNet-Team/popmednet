using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Dns.DTO.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("Terms")]
    public class Term : EntityWithID
    {
        [MaxLength(255)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [MaxLength(100)]
        public string? OID { get; set; }
        [MaxLength(450)]
        public string? ReferenceUrl { get; set; }
        public TermTypes Type { get; set; } = TermTypes.Criteria;
        public virtual ICollection<DataModelSupportedTerm> DataModels { get; set; } = new HashSet<DataModelSupportedTerm>();
        public virtual ICollection<RequestTypeTerm> RequestTypes { get; set; } = new HashSet<RequestTypeTerm>();
    }

    internal class TermConfiguration : IEntityTypeConfiguration<Term>
    {
        public void Configure(EntityTypeBuilder<Term> builder)
        {
            builder.HasIndex(t => t.Name, "IX_Name").IsUnique(false).IsClustered(false);
            builder.HasIndex(t => t.OID, "IX_OID").IsUnique(false).IsClustered(false);
            builder.HasIndex(t => t.ReferenceUrl, "IX_ReferenceUrl").IsUnique(false).IsClustered(false);

            builder.HasMany(t => t.DataModels)
                .WithOne(t => t.Term)
                .IsRequired(true)
                .HasForeignKey(t => t.TermID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.RequestTypes)
                .WithOne(t => t.Term)
                .IsRequired(true)
                .HasForeignKey(t => t.TermID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.Type).HasConversion<int>();
        }
    }

    internal class TermSecurityConfiguration : DnsEntitySecurityConfiguration<Term>
    {
        public override IQueryable<Term> SecureList(DataContext db, IQueryable<Term> query, Utilities.Security.ApiIdentity identity, params DTO.Security.PermissionDefinition[] permissions)
        {
            return query;
        }

        public override Task<bool> CanInsert(DataContext db, Utilities.Security.ApiIdentity identity, params Term[] objs)
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

    internal class TermMappingProfile : AutoMapper.Profile
    {
        public TermMappingProfile()
        {
            CreateMap<Term, DTO.TermDTO>();

        }
    }
}
