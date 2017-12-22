using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.Conditions.Data
{
    public class ICD9StratificationSelectionList

    {
        public readonly static ICD9StratificationSelectionList three = new ICD9StratificationSelectionList("3digit", "3 Digit", 3, 3);
        public readonly static ICD9StratificationSelectionList four = new ICD9StratificationSelectionList("4digit", "4 Digit", 4, 4);
        public readonly static ICD9StratificationSelectionList five = new ICD9StratificationSelectionList("5digit", "5 Digit", 5, 5);

        public readonly static ICD9StratificationSelectionList[] precisionList = { three, four, five };

        public string Name { get; set; }
        public string Display { get; set; }
        public int Precision { get; set; }
        public int Code { get; set; }

        public ICD9StratificationSelectionList(string name, string display, int precision, int code)
        {
            Name = name;
            Display = display;
            Precision = precision;
            Code = code;
        }

        public static string GetName(int? code)
        {
            if (code == null)
                return null;

            foreach (ICD9StratificationSelectionList precision in precisionList)
            {
                if (precision.Code == code)
                    return precision.Name;
            }

            return null;
        }
        
        public static string GetDisplay(int? code)
        {
            if (code == null)
                return null;

            foreach (ICD9StratificationSelectionList precision in precisionList)
            {
                if (precision.Code == code)
                    return precision.Display;
            }

            return null;
        }

        public static int? GetPrecision(int? code)
        {
            if (code == null)
                return null;

            foreach (ICD9StratificationSelectionList precision in precisionList)
            {
                if (precision.Code == code)
                    return (int?)precision.Precision;
            }

            return null;
        }
    }
}
