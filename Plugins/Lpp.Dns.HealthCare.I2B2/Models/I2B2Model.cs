using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using Lpp.Dns.HealthCare.I2B2;
using Lpp.Dns.HealthCare.I2B2.Data.Entities;
using Lpp.Dns.HealthCare.I2B2.Data;
using System.Xml.Serialization;

namespace Lpp.Dns.HealthCare.I2B2.Models
{
    [XmlRootAttribute("I2B2Model", Namespace = "", IsNullable = false)]
    public class I2B2Model
    {
        [XmlIgnore]
        public I2B2RequestType RequestType { get; set; }

        public int? AgeStratification { get; set; }
        public int? SexStratification { get; set; }
        public int? PeriodStratification { get; set; }
        public int? RaceStratification { get; set; }

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
                string[] selectedReports = Report.Split(',');
                foreach (string report in selectedReports)
                {
                    nameList += ReportSelectionList.GetName(Convert.ToInt32(report));
                    if (report != selectedReports.Last())
                        nameList += ",";
                }
                return nameList;
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

        //[XmlElement(ElementName = "stylesheet", Namespace="xsl")]
        //public string XslStylesheet { get; set; }
    }
}
