using Lpp.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Audit
{
    public abstract class ChangeLog : AuditLog
    {
        public EntityState Reason { get; set; }        
    }
}
