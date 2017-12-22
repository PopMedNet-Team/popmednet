using System;
using System.Collections.Generic;

namespace Lpp.Dns.HealthCare.Summary
{
    public partial class DataMartAvailabilityPeriodsLookUp
    {
        public int QueryId { get; set; }
        public int DataMartId { get; set; }
        public int QueryTypeId { get; set; }

        //public Period StartPeriod { get; set; }
        //public Period EndPeriod { get; set; }

        //public PeriodCategory PeriodCategory { get; set; }

        public string PeriodCategory { get; set; }
        public string Period { get; set; }
        public bool IsActive { get; set; }
    }   

    public struct Period
    {
        public int Year { get; set; }
        public int Quarter { get; set; }
    }
}
