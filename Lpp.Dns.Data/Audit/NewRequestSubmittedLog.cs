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
    [Table("LogsNewRequestSubmitted")]
    public class NewRequestSubmittedLog : AuditLog
    {
        public NewRequestSubmittedLog()
        {
            this.EventID = EventIdentifiers.Request.NewRequestSubmitted.ID;
        }

        [Key, Column(Order = 3)]
        public Guid RequestID { get; set; }
        public virtual Request Request { get; set; }

        public Guid? TaskID { get; set; }
        public virtual PmnTask Task { get; set; }

        public Guid? RequestDataMartID { get; set; }
        public virtual RequestDataMart RequestDataMart { get; set; }

        //[Index]
        //public Guid EventID { get; set; }
    }

    internal class NewRequestSubmittedLogDTOMapping : EntityMappingConfiguration<NewRequestSubmittedLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<NewRequestSubmittedLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "New Request Submitted",
                    Message = d.Description
                };
            }
        }
    }
}
