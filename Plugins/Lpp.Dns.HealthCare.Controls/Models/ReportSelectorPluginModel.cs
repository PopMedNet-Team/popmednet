using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Lpp.Dns.HealthCare.Models
{
    public class ReportSelectorPluginModel : DnsPluginModel
    {
        public string Row { get; set; }
        public string Column { get; set; }
        public string Group { get; set; }
        public string Option { get; set; }
    }
}
