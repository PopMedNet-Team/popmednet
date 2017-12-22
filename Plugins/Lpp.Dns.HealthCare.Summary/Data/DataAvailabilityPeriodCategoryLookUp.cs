using System;
using System.Collections.Generic;

namespace Lpp.Dns.HealthCare.Summary
{
    public partial class DataAvailabilityPeriodCategoryLookUp
    {
        public int CategoryTypeId { get; set; }
        public string CategoryType { get; set; }
        public string CategoryDescription { get; set; }
        public bool IsPublished { get; set; }
    }   
}
