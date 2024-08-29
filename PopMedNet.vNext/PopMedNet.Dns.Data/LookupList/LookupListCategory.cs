using PopMedNet.Dns.DTO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("LookupListCategories")]
    public class LookupListCategory
    {
        [Required]
        public Lists ListId { get; set; }
        [Required]
        public int CategoryId { get; set; }

        [MaxLength(500), Column(TypeName = "varchar"), Required]
        public string CategoryName { get; set; }
    }


    internal class LookupListCategoryConfiguration : IEntityTypeConfiguration<LookupListCategory>
    {
        public void Configure(EntityTypeBuilder<LookupListCategory> builder)
        {
            builder.HasKey(e => new { e.ListId, e.CategoryId }).HasName("PK_LookupListCategories");
        }
    }
}
