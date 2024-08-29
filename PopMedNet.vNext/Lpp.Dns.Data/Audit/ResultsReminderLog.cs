using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Events;
using Lpp.Utilities;
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
    [Table("LogsResultsReminder")]
    public class ResultsReminderLog : AuditLog
    {
        public ResultsReminderLog()
        {
            this.EventID = EventIdentifiers.Request.ResultsReminder.ID;
        }

        [Key, Column(Order = 3)]
        public Guid RequestID { get; set; }
        public virtual Request Request { get; set; }
    }

    internal class ResultsReminderLogDTOMapping : EntityMappingConfiguration<ResultsReminderLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<ResultsReminderLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "Results Reminder",
                    Message = d.Description
                };
            }
        }
    }
}
