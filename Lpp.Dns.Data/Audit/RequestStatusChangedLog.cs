using Lpp.Dns.DTO;
using Lpp.Utilities;
using Lpp.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.DTO.Events;

namespace Lpp.Dns.Data.Audit
{
    [Table("LogsRequestStatusChange")]
    public class RequestStatusChangedLog : ChangeLog
    {
        public RequestStatusChangedLog()
        {
            this.EventID = EventIdentifiers.Request.RequestStatusChanged.ID;
        }

        [Key, Column(Order = 3)]
        public Guid RequestID { get; set; }
        public virtual Request Request { get; set; }

        public RequestStatuses OldStatus { get; set; }
        public RequestStatuses NewStatus { get; set; }

        public Guid? TaskID { get; set; }
        public virtual PmnTask Task { get; set; }
        public string EmailBody { get; set; }
        public string MyEmailBody { get; set; }
        public string Subject { get; set; }
    }

    internal class RequestStatusChangedLogDTOMapping : EntityMappingConfiguration<RequestStatusChangedLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<RequestStatusChangedLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "Request Status Changed",
                    Message = d.Description
                };
            }
        }
    }
}
