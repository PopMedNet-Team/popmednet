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
    public class LegacyReportGetModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public Guid DataMartID { get; set; }
        public string OrderBy { get; set; }
    }
}