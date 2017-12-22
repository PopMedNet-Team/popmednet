using Lpp.Dns.DTO.Events;
using Lpp.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Audit
{
    [Table("LogsTaskReminder")]
    public class PmnTaskReminderLog : AuditLog
    {
        public PmnTaskReminderLog()
        {
            this.EventID = EventIdentifiers.Task.WorkflowTaskReminder.ID;
        }

        [Key, Column(Order = 3)]
        public Guid TaskID { get; set; }
        public virtual PmnTask Task { get; set; }
    }
}
