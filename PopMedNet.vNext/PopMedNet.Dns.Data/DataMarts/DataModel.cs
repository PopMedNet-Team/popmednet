using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Utilities.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("DataModels")]
    public class DataModel : EntityWithID
    {
        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool RequiresConfiguration { get; set; }
        public bool QueryComposer { get; set; }

        public virtual ICollection<RequestTypeModel> RequestTypes { get; set; } = new HashSet<RequestTypeModel>();
        public virtual ICollection<DataMartInstalledModel> DataMarts { get; set; } = new HashSet<DataMartInstalledModel>();
        public virtual ICollection<DataModelSupportedTerm> SupportedTerms { get; set; } = new HashSet<DataModelSupportedTerm>();
        public virtual ICollection<DataMart> QueryComposerDataMarts { get; set; } = new HashSet<DataMart>();
    }

    internal class DataModelConfiguration : IEntityTypeConfiguration<DataModel>
    {
        public void Configure(EntityTypeBuilder<DataModel> builder)
        {
            builder.HasMany(t => t.RequestTypes).WithOne(t => t.DataModel).IsRequired(true).HasForeignKey(t => t.DataModelID).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.DataMarts).WithOne(t => t.Model).IsRequired(true).HasForeignKey(t => t.ModelID).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.SupportedTerms).WithOne(t => t.DataModel).IsRequired(true).HasForeignKey(t => t.DataModelID).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.QueryComposerDataMarts).WithOne(t => t.Adapter).IsRequired(false).HasForeignKey(t => t.AdapterID).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(d => d.Name, "IX_Name").IsUnique(false).IsClustered(false);
        }
    }

    public class DataModelMappingProfile : AutoMapper.Profile
    {
        public DataModelMappingProfile()
        {
            CreateMap<DataModel, DTO.DataModelDTO>();
        }
    }
}
