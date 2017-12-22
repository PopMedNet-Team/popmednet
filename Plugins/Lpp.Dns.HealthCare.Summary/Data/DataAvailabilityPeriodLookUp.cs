using System;
using System.Collections.Generic;

namespace Lpp.Dns.HealthCare.Summary
{
    public partial class DataAvailabilityPeriodLookUp
    {
        public int CategoryTypeId { get; set; }
        public string Period { get; set; }
        public bool IsPublished { get; set; }
        public Guid DataMartID { get; set; }
        public Guid ProjectID { get; set; }
    }   
}
