using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.Summary.Data
{
    public class OutputCriteriaSelectionList
    {
        // Making these into Name/Code pairs seems like overkill right now, but we might want them to display "Top 5" instead of "5" someday.
        public readonly static OutputCriteriaSelectionList Top5 = new OutputCriteriaSelectionList("5", 5);
        public readonly static OutputCriteriaSelectionList Top10 = new OutputCriteriaSelectionList("10", 10);
        public readonly static OutputCriteriaSelectionList Top20 = new OutputCriteriaSelectionList("20", 20);
        public readonly static OutputCriteriaSelectionList Top25 = new OutputCriteriaSelectionList("25", 25);
        public readonly static OutputCriteriaSelectionList Top50 = new OutputCriteriaSelectionList("50", 50);
        public readonly static OutputCriteriaSelectionList Top100 = new OutputCriteriaSelectionList("100", 100);

        public readonly static OutputCriteriaSelectionList[] OutputCriteria = { Top5, Top10, Top20, Top25, Top50, Top100 };

        public string Name { get; set; }
        public int Code { get; set; }

        public OutputCriteriaSelectionList(string name, int code)
        {
            Name = name;
            Code = code;
        }

        public static string GetName(int code)
        {
            foreach (OutputCriteriaSelectionList Crit in OutputCriteria)
            {
                if (Crit.Code == code)
                    return Crit.Name;
            }

            return null;
        }

    }

    public partial class OutputCriteriaSelectionLookUp
    {
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
