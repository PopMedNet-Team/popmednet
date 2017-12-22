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
    [Table("LogsRoutingStatusChange")]
    public class RoutingStatusChangeLog : AuditLog
    {
        public RoutingStatusChangeLog()
        {
            this.EventID = EventIdentifiers.Request.RoutingStatusChanged.ID;
        }

        [Key, Column(Order = 3)]
        public Guid RequestDataMartID { get; set; }
        public virtual RequestDataMart RequestDataMart { get; set; }

        public Guid? TaskID { get; set; }
        public virtual PmnTask Task { get; set; }

        public Dns.DTO.Enums.RoutingStatus? OldStatus { get; set; }

        public DTO.Enums.RoutingStatus? NewStatus { get; set; }

    }

    internal class RoutingStatusChangeLogDTOMapping : EntityMappingConfiguration<RoutingStatusChangeLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<RoutingStatusChangeLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "Routing Status Changed",
                    Message = d.Description
                };
            }
        }
    }
}
