using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.ESP
{
    public class ESPQueryResult
    {
        public string PatientID { get; set; }
        public string Sex { get; set; }
        public int? EthnicityCode { get; set; }
        public string Ethnicity { get; set; }
        public string Race { get; set; }
        public string Zip { get; set; }
        public string TobaccoUse { get; set; }

        public string Code { get; set; }
        public string CodeDescription { get; set; }
        
        public string Disease { get; set; }

        public int? ObservationPeriod { get; set; }

        public string Age_5yrGroup { get; set; }
        public string Age_10yrGroup { get; set; }

        public string CenterID { get; set; }

        public int? Age_Detect { get; set; }

        //This will be the grouping count of the patients matching
        public int? Patients { get; set; }
    }
}
