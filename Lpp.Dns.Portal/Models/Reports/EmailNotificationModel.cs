using Lpp.Dns.Data;
using System.Collections.Generic;
using Lpp.Audit.UI;
using System;

namespace Lpp.Dns.Portal.Models
{
    public class EmailNotificationModel
    {
        public User User { get; set; }
        public IEnumerable<VisualizedAuditEvent> Events { get; set; }
    }
}