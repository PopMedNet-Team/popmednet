using System;
using System.ComponentModel.DataAnnotations;

namespace Lpp.Dns.HealthCare.Models
{
    public class ObservationPeriodPluginModel : DnsPluginModel
    {
        [Display(Name = "Start Period")]
        [DataType(DataType.Date)]
        public DateTime StartPeriod { get; set; }

        [Display(Name = "End Period")]
        [DataType(DataType.Date)]
        public DateTime EndPeriod { get; set; }

        [Display(Name = "Use Start Period")]
        public bool UseStartPeriod { get; set; }

        [Display(Name = "Use End Period")]
        public bool UseEndPeriod { get; set; }
    }
}
