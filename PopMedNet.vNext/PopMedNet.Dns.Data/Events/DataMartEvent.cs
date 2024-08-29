using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("DataMartEvents")]
    public class DataMartEvent : BaseEventPermission
    {
        public DataMartEvent() { }

        public Guid DataMartID { get; set; }
        public virtual DataMart? DataMart { get; set; }

        public virtual Event? Event { get; set; }
    }
    internal class DataMartEventConfiguration : IEntityTypeConfiguration<DataMartEvent>
    {
        public void Configure(EntityTypeBuilder<DataMartEvent> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.DataMartID, e.EventID }).HasName("PK_dbo.DataMartEvents");
        }
    }
}
