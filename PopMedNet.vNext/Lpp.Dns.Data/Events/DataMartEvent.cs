using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("DataMartEvents")]
    public class DataMartEvent : BaseEventPermission
    {
        public DataMartEvent() { }

        [Key, Column(Order = 3)]
        public Guid DataMartID { get; set; }
        public virtual DataMart DataMart { get; set; }

        public virtual Event Event { get; set; }
    }
}
