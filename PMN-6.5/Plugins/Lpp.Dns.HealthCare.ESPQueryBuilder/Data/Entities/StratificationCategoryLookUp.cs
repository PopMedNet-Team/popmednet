using System;
using System.Collections.Generic;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder.Data.Entities
{
    public partial class StratificationCategoryLookUp
    {
        public string StratificationType { get; set; }
        public int StratificationCategoryId { get; set; }
        public string CategoryText { get; set; }
        public string ClassificationText { get; set; }
        public string ClassificationFormat { get; set; }
    }   
}
