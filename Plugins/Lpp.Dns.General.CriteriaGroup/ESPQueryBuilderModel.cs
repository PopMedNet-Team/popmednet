using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Data;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Data.Entities;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder.Models
{
    [XmlRootAttribute("ESPQueryBuilderModel", Namespace = "", IsNullable = false)]
    public class ESPQueryBuilderModel
    {
        [XmlIgnore]
        public string RequestName { get; set; }

        [XmlIgnore]
        public ESPQueryBuilderRequestType RequestType { get; set; }

        [XmlIgnore]
        public string CriteriaGroups { get; set; }

        public int? AgeStratification { get; set; }
        public int? SexStratification { get; set; }
        public int? PeriodStratification { get; set; }
        public int? RaceStratification { get; set; }
        public int? ICD9Stratification { get; set; }

        public int Sex { get; set; }
        public string Race { get; set; }
        public string Report { get; set; }

        [XmlAttribute]
        public DateTime StartPeriod { get; set; }

        [XmlAttribute]
        public DateTime EndPeriod { get; set; }

        [XmlAttribute]
        public string Codes { get; set; }

        [XmlAttribute]
        public string Disease { get; set; }

        [XmlAttribute]
        public int MinAge { get; set; }

        [XmlAttribute]
        public int MaxAge { get; set; }

        [XmlAttribute]
        public string Genders
        {
            get
            {
                return SexSelectionList.GetName(Sex);
            }

            set { }
        }

        [XmlAttribute]
        public string Races
        {
            get
            {
                string nameList = "";
                string[] selectedRaces = Race.Split(',');
                foreach (string race in selectedRaces)
                {
                    nameList += RaceSelectionList.GetName(Convert.ToInt32(race));
                    if (race != selectedRaces.Last())
                        nameList += ",";
                }
                return nameList;
            }

            set { }
        }

        [XmlAttribute]
        public string AgeStratificationBy
        {
            get
            {
                return AgeStratificationSelectionList.GetDisplay(AgeStratification);
            }

            set { }
        }

        public int? AgeStratificationByNumberOfYears
        {
            get
            {
                return AgeStratificationSelectionList.GetNumberOfYears(AgeStratification);
            }

            set { }
        }

        [XmlAttribute]
        public string PeriodStratficiationBy
        {
            get
            {
                return PeriodStratificationSelectionList.GetName(PeriodStratification);
            }

            set { }
        }

        [XmlAttribute]
        public string ReportBy
        {
            get
            {
                string nameList = "";
                if (!Report.NullOrEmpty())
                {
                    string[] selectedReports = Report.Split(',');
                    foreach (string report in selectedReports)
                    {
                        nameList += ReportSelectionList.GetName(Convert.ToInt32(report));
                        if (report != selectedReports.Last())
                            nameList += ",";
                    }
                }
                return nameList;
            }

            set { }
        }

        [XmlAttribute]
        public string ICD9StratificationBy
        {
            get
            {
                return ICD9StratificationSelectionList.GetDisplay(ICD9Stratification);
            }

            set { }
        }

        public int? ICD9StratificationByPrecision
        {
            get
            {
                return ICD9StratificationSelectionList.GetPrecision(ICD9Stratification);
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
        public IEnumerable<StratificationCategoryLookUp> PeriodStratifications { get; set; }
        [XmlIgnore]
        public IEnumerable<StratificationCategoryLookUp> RaceStratifications { get; set; }

        [XmlIgnore]
        public IEnumerable<StratificationCategoryLookUp> SexSelections { get; set; }
        [XmlIgnore]
        public IEnumerable<StratificationCategoryLookUp> RaceSelections { get; set; }
        [XmlIgnore]
        public IEnumerable<ESPRequestBuilderSelection> DiseaseSelections { get; set; }
        [XmlIgnore]
        public IEnumerable<ReportSelection> ReportSelections { get; set; }

        [XmlIgnore]
        public TermsModel TermSelections { get; set; }

        //[XmlElement(ElementName = "stylesheet", Namespace="xsl")]
        //public string XslStylesheet { get; set; }
    }

}
