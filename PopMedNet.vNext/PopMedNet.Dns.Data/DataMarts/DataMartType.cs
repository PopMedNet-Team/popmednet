using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Utilities.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("DataMartTypes")]
    public partial class DataMartType : EntityWithID
    {
        [Required, MaxLength(50), Column("DataMartType")]
        public string Name { get; set; }

        public virtual ICollection<DataMart> DataMarts { get; set; } = new HashSet<DataMart>();
    }

    internal class DataMartTypeConfiguration : IEntityTypeConfiguration<DataMartType>
    {
        public void Configure(EntityTypeBuilder<DataMartType> builder)
        {
            builder.HasMany(t => t.DataMarts).WithOne(t => t.DataMartType).IsRequired(true).HasForeignKey(t => t.DataMartTypeID).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
