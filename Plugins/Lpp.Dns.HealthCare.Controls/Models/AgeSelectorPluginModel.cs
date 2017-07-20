using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Lpp.Dns.HealthCare.Models
{
    public class AgeSelectorPluginModel : DnsPluginModel
    {
        [Range(0, 120)]
        public int Age { get; set; }

        [Display(Name = "Operator")]
        public string AgeOperator { get; set; }
        
        [Display(Name = "As of Date")]
        [DataType(DataType.Date)]
        public DateTime AgeAsOfDate { get; set; }

        public string AgeAsOfDatePreset { get; set; }

        static public IEnumerable<SelectListItem> AgeOperators = new List<SelectListItem>() {
            new SelectListItem() { Text = ">",  Value=">"},
            new SelectListItem() { Text = ">=",  Value=">="},
            new SelectListItem() { Text = "=",  Value="="},
            new SelectListItem() { Text = "<",  Value="<"},
            new SelectListItem() { Text = "<=",  Value="<="}
        };
    }
}
