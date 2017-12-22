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
    [Table("LogsSubmittedRequestNeedsApproval")]
    public class SubmittedRequestNeedsApprovalLog : AuditLog
    {
        public SubmittedRequestNeedsApprovalLog()
        {
            this.EventID = EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID;
        }

        [Key, Column(Order = 3)]
        public Guid RequestID { get; set; }
        public virtual Request Request { get; set; }

        public Guid? TaskID { get; set; }
        public virtual PmnTask Task { get; set; }
    }

    internal class SubmittedRequestNeedsApprovalLogDTOMapping : EntityMappingConfiguration<SubmittedRequestNeedsApprovalLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<SubmittedRequestNeedsApprovalLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "Submitted Request Needs Approval",
                    Message = d.Description
                };
            }
        }
    }
}
