using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters
{
    public class AgeRangeValues
    {
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public DateTimeOffset? BirthStartDate { get; set; }
        public DateTimeOffset? BirthEndDate { get; set; }
        public DTO.Enums.AgeRangeCalculationType? CalculationType { get; set; }
        public DateTime? CalculateAsOf { get; set; }
    }
}
