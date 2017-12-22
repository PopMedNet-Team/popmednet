using System;
using System.Collections.Generic;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public class SummaryRequestModel
    {
        public int? AgeStratification { get; set; }
        public int? SexStratification { get; set; }

        public int SubtypeId { get; set; }
        public string Codes { get; set; }
        public string MetricType { get; set; }
        public string OutputCriteria { get; set; }
        public string Setting { get; set; }
        public DTO.Enums.Coverages? Coverage { get; set; }
        public string Period { get; set; }
        public string[] CodeNames { get; set; }
        public string StartPeriod { get; set; }
        public string EndPeriod { get; set; }
        public string StartQuarter { get; set; }
        public string EndQuarter { get; set; }

        public string Sex { get; set; }
        public string AgeStratificationBy { get; set; }
    }
}
