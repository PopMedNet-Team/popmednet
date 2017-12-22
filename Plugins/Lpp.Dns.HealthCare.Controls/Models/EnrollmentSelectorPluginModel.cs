using System.ComponentModel.DataAnnotations;

namespace Lpp.Dns.HealthCare.Models
{
    public class EnrollmentSelectorPluginModel : DnsPluginModel
    {
        public int Prior { get; set; }
        public int After { get; set; }

        [Display(Name = "Continuous Enrollment")]
        public bool Continuous { get; set; }
    }
}
