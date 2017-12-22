using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.I2B2.Data
{
    public class ReportSelectionList
    {
        public readonly static ReportSelectionList age = new ReportSelectionList("AgeStratification", "Age", 1);
        public readonly static ReportSelectionList sex = new ReportSelectionList("SexStratification", "Sex", 2);
        public readonly static ReportSelectionList period = new ReportSelectionList("PeriodStratification", "Period", 3);
        public readonly static ReportSelectionList race = new ReportSelectionList("RaceStratification", "Race", 4);
        public readonly static ReportSelectionList center = new ReportSelectionList("CenterStratification", "Center", 5);

        public readonly static ReportSelectionList[] reports = { age, sex, period, race, center };

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
}
