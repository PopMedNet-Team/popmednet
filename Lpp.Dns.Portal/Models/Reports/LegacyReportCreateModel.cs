using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using Lpp.Dns.Portal.Controllers;
using Lpp.Audit;
using Lpp.Audit.UI;

namespace Lpp.Dns.Portal.Models
{
    public class LegacyReportCreateModel
    {
        public IEnumerable<DataMart> DataMarts { get; set; }
        public IEnumerable<string> OrderBy { get; set; }
    }
}