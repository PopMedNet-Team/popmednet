using PopMedNet.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("RequestDataMartResponseSearchResults")]
    public class ResponseSearchResult : Entity
    {
        [Column("RequestDataMartResponseID")]
        public Guid ResponseID { get; set; }
        public virtual Response? Response { get; set; }
        public Guid ItemID { get; set; }
    }


    internal class ResponseSearchResultConfiguration : IEntityTypeConfiguration<ResponseSearchResult>
    {
        public void Configure(EntityTypeBuilder<ResponseSearchResult> builder)
        {
            builder.HasKey(e => new { e.ResponseID, e.ItemID }).HasName("PK_dbo.RequestDataMartResponseSearchResults");
        }
    }
}
