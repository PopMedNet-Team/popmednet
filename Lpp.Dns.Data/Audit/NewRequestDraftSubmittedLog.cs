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

    [Table("LogsNewRequestDraftSubmitted")]
    public class NewRequestDraftSubmittedLog : AuditLog
    {
        public NewRequestDraftSubmittedLog()
        {
            this.EventID = EventIdentifiers.Request.NewRequestDraftSubmitted.ID;
        }

        [Key, Column(Order = 3)]
        public Guid RequestID { get; set; }
        public virtual Request Request { get; set; }

    }

    internal class NewRequestDraftSubmittedLogDTOMapping : EntityMappingConfiguration<NewRequestDraftSubmittedLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<NewRequestDraftSubmittedLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "New Request Draft Submitted",
                    Message = d.Description
                };
            }
        }
    }
    
}
