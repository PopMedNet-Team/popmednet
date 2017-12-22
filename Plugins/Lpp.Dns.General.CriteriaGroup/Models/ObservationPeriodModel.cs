using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Lpp.Dns.General.CriteriaGroup.Models
{
    public class ObservationPeriodModel
    {
        public DateTime? StartPeriod { get; set; }
        public DateTime? EndPeriod { get; set; }
    }

}
