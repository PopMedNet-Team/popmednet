using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.I2B2.Data
{
    public class PeriodStratificationSelectionList
    {
        public readonly static PeriodStratificationSelectionList monthly = new PeriodStratificationSelectionList("Monthly", 1);
        public readonly static PeriodStratificationSelectionList yearly = new PeriodStratificationSelectionList("Yearly", 2);

        public readonly static PeriodStratificationSelectionList[] periods = { monthly, yearly };

        public string Name { get; set; }
        public int Code { get; set; }

        public PeriodStratificationSelectionList(string name, int code)
        {
            Name = name;
            Code = code;
        }

        public static string GetName(int? code)
        {
            if (code == null)
                return null;

            foreach (PeriodStratificationSelectionList period in periods)
            {
                if (period.Code == code)
                    return period.Name;
            }

            return null;
        }
        
    }
}
