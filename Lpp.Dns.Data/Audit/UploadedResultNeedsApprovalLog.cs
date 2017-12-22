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
    [Table("LogsUploadedResultNeedsApproval")]
    public class UploadedResultNeedsApprovalLog : AuditLog
    {
        public UploadedResultNeedsApprovalLog()
        {
            this.EventID = EventIdentifiers.Response.UploadedResultNeedsApproval.ID;
        }

        [Key, Column(Order = 3)]
        public Guid RequestDataMartID { get; set; }
        public virtual RequestDataMart RequestDataMart { get; set; }

        public Guid? TaskID { get; set; }
        public virtual PmnTask Task { get; set; }
    }

    internal class UploadedResultNeedsApprovalLogDTOMapping : EntityMappingConfiguration<UploadedResultNeedsApprovalLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<UploadedResultNeedsApprovalLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "Uploaded Result Needs Approval",
                    Message = d.Description
                };
            }
        }
    }
}
