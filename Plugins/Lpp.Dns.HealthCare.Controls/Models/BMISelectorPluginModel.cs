using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Lpp.Dns.HealthCare.Models
{
    public class BMISelectorPluginModel : DnsPluginModel
    {
        public string BMI { get; set; }
        public string BMIOption { get; set; }
    }
}
