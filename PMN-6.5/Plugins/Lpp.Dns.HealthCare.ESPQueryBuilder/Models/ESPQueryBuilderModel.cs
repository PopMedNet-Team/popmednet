using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Xml.Serialization;
using Lpp.Dns.General.CriteriaGroup.Models;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Data;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Data.Entities;
using Lpp.Dns.Data;
using Lpp.Utilities;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder.Models
{
    [XmlRootAttribute("ESPQueryBuilderModel", Namespace = "", IsNullable = false)]
    public class ESPQueryBuilderModel
    {
        [XmlIgnore]
        public string RequestName { get; set; }

        [XmlIgnore]
        public ESPQueryBuilderRequestType RequestType { get; set; }

        public string CriteriaGroupsJSON { get; set; }

        [XmlIgnore]
        public IEnumerable<CriteriaGroupModel> CriteriaGroups 
        { 
            get
            {
                IList<CriteriaGroupModel> criteriaGroups = new List<CriteriaGroupModel>();

                if (CriteriaGroupsJSON.IsNullOrEmpty())
                    return criteriaGroups;

                foreach (var criteriaGroup in Json.Decode(CriteriaGroupsJSON))
                {
                    IList<TermModel> terms = new List<TermModel>();
                    CriteriaGroupModel cgm = new CriteriaGroupModel
                    {
                        Terms = terms
                    };
                    cgm.CriteriaGroupName = criteriaGroup["CriteriaGroupName"];
                    cgm.ExcludeCriteriaGroup = criteriaGroup["ExcludeCriteriaGroup"];
                    cgm.SaveCriteriaGroup = criteriaGroup["SaveCriteriaGroup"];
                    foreach (var term in criteriaGroup["Terms"])
                    {
                        TermModel tm = new TermModel();
                        foreach (var arg in term)
                        {
                            if (arg["name"] == "TermName")
                                tm.TermName = arg["value"];
                            else if (((string)arg["name"]).StartsWith("Codes"))
                                tm.Args.Add("Codes", arg["value"]);
                            else if (!tm.Args.ContainsKey(arg["name"]))
                                tm.Args.Add(arg["name"], arg["value"]);
                        }
                        terms.Add(tm);
                    }

                    criteriaGroups.Add(cgm);
                }

                return criteriaGroups;
            }
        }

        #region Existing or Selected View Values Serialized as Element Values

        public int? AgeStratification { get; set; }
        public int? SexStratification { get; set; }
        public int? PeriodStratification { get; set; }
        public int? RaceStratification { get; set; }
        public int? SmokingStratification { get; set; }
        public int? EthnicityStratification { get; set; }
        public int? ICD9Stratification { get; set; }

        public int Sex { get; set; }
        public string Race { get; set; }
        public string Smoking { get; set; }
        public string Ethnicity { get; set; }
        public string Report { get; set; }

        #endregion

        #region Existing or Selected View Values Serialized as Attributes

        [XmlAttribute] 
        public string StartPeriod { get; set; }
        [XmlAttribute] 
        public string EndPeriod { get; set; }
        [XmlIgnore] 
        public DateTime? StartPeriodDate
            { 
                get
                {
                    DateTime dt;
                    //Prior serialized datetime are in format 'yyyy-mm-dd T00:00:00' trim the time part as this format cannot be parsed as datetime.
                    if (!string.IsNullOrEmpty(StartPeriod) && StartPeriod.Contains("T")) 
                    {
                        StartPeriod = StartPeriod.Substring(0, StartPeriod.IndexOf("T"));
                    }
                    else if (string.IsNullOrWhiteSpace(StartPeriod))
                    {
                        return null;
                    }

                    DateTime.TryParse(StartPeriod, out dt);
                    return dt;
                } 
                set 
                {
                    if (value == null)
                    {
                        StartPeriod = "";
                    } else {
                        StartPeriod = value.Value.ToString("yyyy-MM-dd");
                    }
                } 
            }
        
        [XmlIgnore] 
        public DateTime? EndPeriodDate
        {
            get
            {
                DateTime dt;
                //Prior serialized datetime are in format 'yyyy-mm-dd T00:00:00' trim the time part as this format cannot be parsed as datetime.
                if (!string.IsNullOrEmpty(EndPeriod) && EndPeriod.Contains("T"))
                {
                    EndPeriod = EndPeriod.Substring(0, EndPeriod.IndexOf("T"));
                }
                else if (string.IsNullOrWhiteSpace(EndPeriod))
                {
                    return null;
                }
                DateTime.TryParse(EndPeriod, out dt);
                return dt;
            }
            set
            {
                if (value == null)
                {
                    EndPeriod = "";
                }
                else
                {
                    EndPeriod = value.Value.ToString("yyyy-MM-dd");
                }
            }
        }
        [XmlAttribute] 
        public string Codes { get; set; }
        [XmlAttribute] 
        public string Disease { get; set; }
        [XmlAttribute] 
        public string MinAge { get; set; }
        [XmlAttribute] 
        public string MaxAge { get; set; }

        [XmlAttribute] 
        public string ZipCodes { get; set; }
        [XmlAttribute] 
        public int MinVisits { get; set; }


        [XmlIgnore]
        public string LocationNames { get; set; }
        [XmlIgnore]
        public string LocationCodes { get; set; }

        #endregion

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
        public string DiseaseDisplay
        {
            get
            {
                return DiseaseSelectionList.diseases.Where(d => String.Equals(d.Name, Disease, StringComparison.OrdinalIgnoreCase)).Select(d => d.Display).FirstOrDefault();
            }

            set { }
        }

        [XmlAttribute]
        public string Races
        {
            get
            {
                string nameList = "";
                if (Race.IsNullOrEmpty())
                    return null;

                string[] selectedRaces = Race.Split(',');
                foreach (string race in selectedRaces)
                {
                    int index;
                    if (int.TryParse(race, out index))
                    {
                        nameList += RaceSelectionList.GetName(index);
                        if (race != selectedRaces.Last())
                            nameList += ",";
                    }
                    
                }
                return nameList.Distinct().ToString();
            }

            set { }
        }

        [XmlAttribute]
        public string Smokings
        {
            get
            {
                string nameList = "";
                if (Smoking.IsNullOrEmpty())
                    return null;

                List<string> selectedSmoking = Smoking.Split(',').ToList();

                foreach (string smoking in selectedSmoking.Distinct())
                {
                    int index;
                    if (int.TryParse(smoking, out index))
                    {
                        nameList += SmokingSelectionList.GetName(index);
                        if (smoking != selectedSmoking.Last())
                            nameList += ",";
                    }

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

        public bool TobaccoUse
        {
            get;
            set;
        }

        [XmlAttribute]
        public string ReportBy
        {
            get
            {
                string nameList = "";
                if (!Report.IsNullOrEmpty())
                {
                    string[] selectedReports = Report.Split(',');
                    foreach (string report in selectedReports)
                    {
                        int index;
                        if (int.TryParse(report, out index))
                        {
                            nameList += ReportSelectionList.GetName(index);
                            if (report != selectedReports.Last())
                                nameList += ",";
                        }
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

        #region Full Selection Lists to Populate View Dropdowns

        [XmlIgnore] 
        public IEnumerable<StratificationCategoryLookUp> SexSelections { get; set; }
        [XmlIgnore] 
        public IEnumerable<StratificationCategoryLookUp> RaceSelections { get; set; }
        [XmlIgnore] 
        public IEnumerable<StratificationCategoryLookUp> SmokingSelections { get; set; }
        [XmlIgnore] 
        public IEnumerable<StratificationCategoryLookUp> EthnicitySelections { get; set; }
        [XmlIgnore] 
        public IEnumerable<ESPRequestBuilderSelection> DiseaseSelections { get; set; }
        [XmlIgnore] 
        public IEnumerable<ReportSelection> ReportSelections { get; set; }
        [XmlIgnore] 
        public IEnumerable<StratificationCategoryLookUp> ZipCodeSelections { get; set; }
        [XmlIgnore] 
        public TermSelectionsModel TermSelections { get; set; }

        [XmlIgnore] 
        public IEnumerable<LookupListCategory> ICD9Categories { get; set; }
        [XmlIgnore] 
        public IEnumerable<LookupListValue> ICD9Values { get; set; }
        [XmlIgnore] 
        public IEnumerable<LookupListCategory> ZipCategories { get; set; }
        [XmlIgnore] 
        public IEnumerable<LookupListValue> ZipValues { get; set; }

        #endregion
    }

}
