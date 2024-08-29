using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Dns.DTO.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("TemplateTerms")]
    public class TemplateTerm
    {
        public Guid TemplateID { get; set; }
        public virtual Template? Template { get; set; }
        public Guid TermID { get; set; }
        public virtual Term? Term { get; set; }
        public bool Allowed { get; set; }
        public QueryComposerSections Section { get; set; }
    }
    internal class TemplateTermConfiguration : IEntityTypeConfiguration<TemplateTerm>
    {
        public void Configure(EntityTypeBuilder<TemplateTerm> builder)
        {
            builder.HasKey(e => new { e.TemplateID, e.TermID, e.Section }).HasName("PK_dbo.TemplateTerms");
            builder.Property(e => e.Section).HasConversion<int>();
        }
    }
}
