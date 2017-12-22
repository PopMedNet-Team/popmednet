using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Lpp.Dns.DataMart.Model
{
    [XmlRootAttribute("SummaryRequestModel", Namespace = "", IsNullable = false)]
    public class SummaryRequestModel
    {
        public int? AgeStratification { get; set; }
        public int? SexStratification { get; set; }

        public int SubtypeId { get; set; }
        [XmlAttribute] public string Codes { get; set; }
        [XmlAttribute] public string MetricType { get; set; }
        [XmlAttribute] public string OutputCriteria { get; set; }
        [XmlAttribute] public string Setting { get; set; }
        [XmlAttribute] public string Coverage { get; set; }
        [XmlAttribute] public string Period { get; set; }
        public string[] CodeNames { get; set; }
        public string StartPeriod { get; set; }
        public string EndPeriod { get; set; }
        public string StartQuarter { get; set; }
        public string EndQuarter { get; set; }

        public string Gender { get; set; }
        public string AgeStratificationBy { get; set; }
    }
}
