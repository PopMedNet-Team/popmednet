using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Audit
{
    [Table("LogsTaskChange")]
    public class PmnTaskChangeLog : ChangeLog
    {
        public PmnTaskChangeLog()
        {
            this.EventID = Lpp.Dns.DTO.Events.EventIdentifiers.Task.Change.ID;
        }

        [Key, Column(Order=3)]
        public Guid TaskID { get; set; }
        public virtual PmnTask Task { get; set; }
    }
}
