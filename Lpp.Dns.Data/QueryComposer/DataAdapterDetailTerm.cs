using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
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
        [Key, Column(Order = 1)]
        public DTO.Enums.QueryComposerQueryTypes QueryType { get; set; }
        /// <summary>
        /// Gets or sets the Term ID.
        /// </summary>
        [Key, Column(Order = 2)]
        public Guid TermID { get; set; }
        /// <summary>
        /// Gets or sets the Term.
        /// </summary>
        public virtual Term Term { get; set; }

    }

    internal class DataAdapterDetailTermConfiguration : EntityTypeConfiguration<DataAdapterDetailTerm> {
    }

}
