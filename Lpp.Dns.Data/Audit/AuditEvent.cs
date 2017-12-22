using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("AuditEvents")]
    public class AuditEvent : BaseObject<int>
    {
        public Guid KindID { get; set; }
        public DateTime Time { get; set; }
        public Guid TargetID1 { get; set; }
        public Guid TargetID2 { get; set; }
        public Guid TargetID3 { get; set; }
        public Guid TargetID4 { get; set; }
        public Guid TargetID5 { get; set; }
        public Guid TargetID6 { get; set; }
        public Guid TargetID7 { get; set; }
        public Guid TargetID8 { get; set; }
        public Guid TargetID9 { get; set; }
        public Guid TargetID10 { get; set; }

        public virtual ICollection<AuditPropertyValue> Properties {get; set;}
    }

    internal class AuditEventConfiguration : EntityTypeConfiguration<AuditEvent>
    {
        public AuditEventConfiguration()
        {
            //Define the actual column name for the inherited property. This will be removed once the naming scheme is corrected in the DB.
            HasMany(t => t.Properties).WithRequired(t => t.AuditEvent).HasForeignKey(t => t.AuditEventID).WillCascadeOnDelete(true);
        }
    }

}
