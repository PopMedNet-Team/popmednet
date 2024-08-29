using System;
using System.Collections.Generic;

namespace Lpp.Dns.HealthCare
{
    public partial class LookupListValue
    {
        public int ListId { get; set; }
        public int CategoryId { get; set; }
        public virtual LookupListCategory Category { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemCodeWithNoPeriod { get; set; }
    }

    public partial class LookupListValueSearchResult
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemCodeWithNoPeriod { get; set; }
    }
}