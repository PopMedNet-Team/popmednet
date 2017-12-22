using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using Lpp.Dns.HealthCare.Summary.Data;
using System.Xml.Serialization;

namespace Lpp.Dns.HealthCare.Summary.Models
{
    public class SummaryRequestViewModel
    {
        public SummaryRequestModel Base { get; set; }
        public IEnumerable<Lpp.Dns.Data.LookupListValue> Codes { get; set; }
        public string Codeses { get; set; }
    }
}
