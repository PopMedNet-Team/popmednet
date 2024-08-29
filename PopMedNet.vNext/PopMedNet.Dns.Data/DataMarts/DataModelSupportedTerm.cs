using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("DataModelSupportedTerms")]
    public class DataModelSupportedTerm : Entity
    {
        public Guid DataModelID { get; set; }
        public virtual DataModel? DataModel { get; set; }
        public Guid TermID { get; set; }
        public virtual Term? Term { get; set; }
    }
    internal class DataModelSupportedTermConfiguration : IEntityTypeConfiguration<DataModelSupportedTerm>
    {
        public void Configure(EntityTypeBuilder<DataModelSupportedTerm> builder)
        {
            builder.HasKey(e => new { e.DataModelID, e.TermID }).HasName("PK_dbo.DataModelSupportedTerms");
        }
    }
}
