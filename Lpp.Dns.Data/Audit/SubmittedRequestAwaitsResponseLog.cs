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
    [Table("LogsSubmittedRequestAwaitsResponse")]
    public class SubmittedRequestAwaitsResponseLog : AuditLog
    {
        public SubmittedRequestAwaitsResponseLog()
        {
            this.EventID = EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID;
        }

        [Key, Column(Order = 3)]
        public Guid RequestID { get; set; }
        public virtual Request Request { get; set; }

        public Guid? TaskID { get; set; }
        public virtual PmnTask Task { get; set; }
    }

    internal class SubmittedRequestAwaitsResponseLogDTOMapping : EntityMappingConfiguration<SubmittedRequestAwaitsResponseLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<SubmittedRequestAwaitsResponseLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "Submitted Request Awaits Response",
                    Message = d.Description
                };
            }
        }
    }
}
