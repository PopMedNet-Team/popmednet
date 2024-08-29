using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Dns.DTO.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    /// <summary>
    /// The relation between a DataAdapterDetail (QueryType) and a Term.
    /// </summary>
    [Table("DataAdapterDetailTerms")]
    public class DataAdapterDetailTerm
    {
        /// <summary>
        /// Gets or sets the QueryType.
        /// </summary>
        public QueryComposerQueryTypes QueryType { get; set; }
        /// <summary>
        /// Gets or sets the Term ID.
        /// </summary>
        public Guid TermID { get; set; }
        /// <summary>
        /// Gets or sets the Term.
        /// </summary>
        public virtual Term? Term { get; set; }

    }


    internal class DataAdapterDetailTermConfiguration : IEntityTypeConfiguration<DataAdapterDetailTerm>
    {
        public void Configure(EntityTypeBuilder<DataAdapterDetailTerm> builder)
        {
            builder.HasKey(e => new { e.QueryType, e.TermID }).HasName("PK_dbo.DataAdapterDetailTerms");
        }
    }
}
