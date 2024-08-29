using Microsoft.EntityFrameworkCore;
using PopMedNet.Utilities.Logging;

namespace PopMedNet.Dns.Data.Audit
{
    public abstract class ChangeLog : AuditLog
    {
        public EntityState Reason { get; set; }
    }
}
