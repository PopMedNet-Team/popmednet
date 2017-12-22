using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Events;
using Lpp.Utilities;
using Lpp.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Audit
{
    [Table("LogsNewDataMartClient")]
    public class NewDataMartClientLog : AuditLog
    {
        public NewDataMartClientLog()
        {
            this.EventID = EventIdentifiers.DataMart.NewDataMartAvailable.ID;
        }

        public DateTime LastModified { get; set; }
    }

    internal class NewDataMartClientLogDTOMapping : EntityMappingConfiguration<NewDataMartClientLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<NewDataMartClientLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "New DataMart Client",
                    Message = d.Description
                };
            }
        }
    }
}
