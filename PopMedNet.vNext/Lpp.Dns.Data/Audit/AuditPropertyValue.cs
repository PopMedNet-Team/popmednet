using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("AuditPropertyValues")]
    public class AuditPropertyValue : BaseObject<int>
    {
        public AuditPropertyValue()
        {
            DoubleValue = 0;
            IntValue = 0;
            DateTimeValue = DateTime.Now;
            GuidValue = Guid.Empty;
        }

        [Column("EventID")]
        public int AuditEventID { get; set; }
        public virtual AuditEvent AuditEvent { get; set; }

        public Guid PropertyID { get; set; }

        public int? IntValue { get; set; }

        public string StringValue { get; set; }

        public double? DoubleValue { get; set; }

        public DateTime? DateTimeValue { get; set; }

        public Guid? GuidValue { get; set; }
    }
}
