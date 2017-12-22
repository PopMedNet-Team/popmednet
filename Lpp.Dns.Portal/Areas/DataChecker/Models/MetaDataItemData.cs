using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.Root.Areas.DataChecker.Models
{
    public class MetaDataItemData
    {
        public string DP { get; set; }

        public int? ETL { get; set; }

        public DateTime? DIA_MIN { get; set; }
        public DateTime? DIA_MAX { get; set; }

        public DateTime? DIS_MIN { get; set; }
        public DateTime? DIS_MAX { get; set; }

        public DateTime? ENC_MIN { get; set; }
        public DateTime? ENC_MAX { get; set; }

        public DateTime? ENR_MIN { get; set; }
        public DateTime? ENR_MAX { get; set; }

        public DateTime? PRO_MIN { get; set; }
        public DateTime? PRO_MAX { get; set; }

        public DateTime? DP_MIN { get; set; }
        public DateTime? DP_MAX { get; set; }

        public DateTime? MSDD_MIN { get; set; }
        public DateTime? MSDD_MAX { get; set; }
    }
}