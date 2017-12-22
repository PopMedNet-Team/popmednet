using Lpp.Dns.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;

namespace Lpp.Dns.Data
{
    [Table("DataMartTypes")]
    public partial class DataMartType : EntityWithID
    {
        [Required, MaxLength(50), Column("DataMartType")]
        public string Name { get; set; }

        public virtual ICollection<DataMart> DataMarts { get; set; }
    }

    internal class DataMartTypeConfiguration : EntityTypeConfiguration<DataMartType>
    {
        public DataMartTypeConfiguration()
        {
            HasMany(t => t.DataMarts).WithRequired(t => t.DataMartType).HasForeignKey(t => t.DataMartTypeID).WillCascadeOnDelete(true);
        }
    }
}
