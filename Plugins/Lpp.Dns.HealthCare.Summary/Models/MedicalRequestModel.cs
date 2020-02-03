using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;

namespace Lpp.Dns.Models.Medical.Models
{
    public class MedicalRequestModel
    {
        public SummaryRequestType RequestType { get; set; }
        public int SubtypeId { get; set; }
        public string Codes { get; set; }
        public int? AgeStratification { get; set; }
        public IEnumerable<StratificationCategoryLookUp> Stratifications { get; set; }
    }
}
