using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Model;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using Lpp.Dns.Portal.Controllers;
using Lpp.Audit;
using Lpp.Audit.UI;

namespace Lpp.Dns.Portal.Models
{
    public class ReportModel
    {
        public IEnumerable<IAuditProperty> Columns { get; set; }
        public IEnumerable<VisualizedAuditEvent> Rows { get; set; }
    }
}