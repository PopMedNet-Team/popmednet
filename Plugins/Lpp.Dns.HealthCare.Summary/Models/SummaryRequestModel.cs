using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using Lpp.Dns.HealthCare.Summary.Data;
using System.Xml.Serialization;
using System.ComponentModel;
//using Lpp.Data;

namespace Lpp.Dns.HealthCare.Summary.Models
{
    [XmlRootAttribute("SummaryRequestModel", Namespace = "", IsNullable = false)]
    public class SummaryRequestModel
    {
        [XmlIgnore]
        public SummaryRequestType RequestType { get; set; }

        public int? AgeStratification { get; set; }
        public int? SexStratification { get; set; }

        public int SubtypeId { get; set; }

        [XmlAttribute, Lpp.Objects.ValidationAttributes.SkipHttpValidation] 
        public string Codes { get; set; }
        [XmlAttribute] public Lpp.Dns.DTO.Enums.Metrics MetricType { get; set; }
        [XmlAttribute] public int OutputCriteria { get; set; }
        [XmlAttribute] public string Setting { get; set; }
        [XmlAttribute] public string Coverage { get; set; }
        [XmlAttribute] public string Period { get; set; }
        public string[] CodeNames { get; set; }
        public string ByYearsOrQuarters { get; set; }
        public string StartPeriod { get; set; }
        public string EndPeriod { get; set; }
        public string StartQuarter { get; set; }
        public string EndQuarter { get; set; }

        [XmlAttribute]
        public string Gender
        {
            get
            {
                switch (SexStratification)
                {
                    case 1:
                        return "Female Only";
                    case 2:
                        return "Male Only";
                    case 3:
                        return "Male and Female";
                    case 4:
                        return "Male and Female Aggregated";
                    default:
                        return "";
                }
            }

            set { }
        }

        [XmlAttribute]
        public string AgeStratificationBy
        {
            get
            {
                switch (AgeStratification)
                {
                    case 1:
                        return "10 Stratifications";
                    case 2:
                        return "7 Stratifications";
                    case 3:
                        return "4 Stratifications";
                    case 4:
                        return "2 Stratifications";
                    default:
                        return "";
                }
            }

            set { }
        }

        [XmlIgnore]
        public IEnumerable<StratificationCategoryLookUp> Stratifications { get; set; }
        [XmlIgnore]
        public IEnumerable<StratificationCategoryLookUp> AgeStratifications { get; set; }
        [XmlIgnore]
        public IEnumerable<StratificationCategoryLookUp> SexStratifications { get; set; }
        [XmlIgnore]
        public IEnumerable<Lpp.Dns.DTO.Enums.Metrics> MetricTypes { get; set; }
        [XmlIgnore]
        public IEnumerable<OutputCriteriaSelectionLookUp> OutputCriteriaList { get; set; }
        [XmlIgnore]
        public IEnumerable<SettingSelectionLookUp> Settings { get; set; }
        [XmlIgnore]
        public IEnumerable<CoverageSelectionLookUp> Coverages { get; set; }

        // No longer using this table. Using distinct union of DataMartAvailabilityPeriods instead.
        //public IEnumerable<DataAvailabilityPeriodLookUp> DataAvailabilityPeriods { get; set; }

        [XmlIgnore]
        public IEnumerable<DataAvailabilityPeriodCategoryLookUp> DataAvailabilityPeriodCategories { get; set; }

        [XmlIgnore]
        public IEnumerable<DataAvailabilityPeriodLookUp> YearsOrQuartersDataAvailabilityPeriods { get; set; }
        [XmlIgnore]
        public IEnumerable<DataAvailabilityPeriodLookUp> YearsDataAvailabilityPeriods { get; set; }
        [XmlIgnore]
        public IEnumerable<DataAvailabilityPeriodLookUp> QuartersDataAvailabilityPeriods { get; set; }
    }
}
