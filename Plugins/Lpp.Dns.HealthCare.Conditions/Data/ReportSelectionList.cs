using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.Conditions.Data
{
    public class ReportSelectionList
    {
        public readonly static ReportSelectionList age = new ReportSelectionList("AgeStratification", "Age", (int)ReportSelectionCode.Age);
        public readonly static ReportSelectionList sex = new ReportSelectionList("SexStratification", "Sex", (int)ReportSelectionCode.Sex);
        public readonly static ReportSelectionList period = new ReportSelectionList("PeriodStratification", "Period", (int)ReportSelectionCode.Period);
        public readonly static ReportSelectionList race = new ReportSelectionList("RaceStratification", "Race", (int)ReportSelectionCode.Race);
        public readonly static ReportSelectionList center = new ReportSelectionList("CenterStratification", "Center", (int)ReportSelectionCode.Center);
        public readonly static ReportSelectionList zip = new ReportSelectionList("ZipStratification", "Zip", (int)ReportSelectionCode.Zip);
        public readonly static ReportSelectionList ethnicity = new ReportSelectionList("EthnicityStratification", "Ethnicity", (int)ReportSelectionCode.Ethnicity);

        public readonly static ReportSelectionList[] reports = { age, sex, period, race, center, zip, ethnicity };

        public string Name { get; set; }
        public string Display { get; set; }
        public int Code { get; set; }

        public ReportSelectionList(string name, string display, int code)
        {
            Name = name;
            Display = display;
            Code = code;
        }

        public static string GetName(int? code)
        {
            if (code == null)
                return null;

            foreach (ReportSelectionList report in reports)
            {
                if (report.Code == code)
                    return report.Name;
            }

            return null;
        }
        
        public static string GetDisplay(int? code)
        {
            if (code == null)
                return null;

            foreach (ReportSelectionList report in reports)
            {
                if (report.Code == code)
                    return report.Display;
            }

            return null;
        }
    }

    // HACK ALERT:
    // So, the model calls for the value to be a number, and those numbers become row ids when this is turned into a grid, and those ids 
    // (which were 1-7 until I changed it to a higher range) were also used by the breadcrumb control and the criteria groups in
    // the Query Composer.  Hence, at least bumping these up to a range of 100+ should avoid those conflicts in the HTML
    public enum ReportSelectionCode
    {
        Age = 101,
        Sex = 102,
        Period = 103,
        Race = 104,
        Center = 105,
        ICD9 = 106,
        Disease = 107,
        Zip = 108,
        TobaccoUse = 109,
        Ethnicity = 110
    }
}
