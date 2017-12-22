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
using System.Data.Entity;


namespace Lpp.Dns.Data.Audit
{
    [Table("LogsRequestDataMartAddedRemoved")]
    public class RequestDataMartAddedRemovedLog : AuditLog
    {

        public RequestDataMartAddedRemovedLog()
        {
            this.EventID = EventIdentifiers.Request.RequestDataMartAddedRemoved.ID;
        }

        [Key, Column(Order = 3)]
        public Guid RequestDataMartID { get; set; }
        public virtual RequestDataMart RequestDataMart { get; set; }

        public Guid? TaskID { get; set; }
        public virtual PmnTask Task { get; set; }

        public EntityState Reason { get; set; } 

    }

    internal class RequestDataMartAddedRemovedLogDTOMapping : EntityMappingConfiguration<RequestDataMartAddedRemovedLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<RequestDataMartAddedRemovedLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "Request DataMart Change",
                    Message = d.Description
                };
            }
        }
    }
}
