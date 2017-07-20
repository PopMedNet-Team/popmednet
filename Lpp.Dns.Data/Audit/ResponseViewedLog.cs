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
    [Table("LogsResponseViewed")]
    public class ResponseViewedLog : AuditLog
    {
        public ResponseViewedLog()
        {
            this.EventID = EventIdentifiers.Response.ResultsViewed.ID;
        }

        [Key, Column(Order = 3)]
        public Guid ResponseID { get; set; }
        public virtual Response Response { get; set; }
    }

    internal class ResponseViewedLogDTOMapping : EntityMappingConfiguration<ResponseViewedLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<ResponseViewedLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "Response Viewed",
                    Message = d.Description
                };
            }
        }
    }
}
