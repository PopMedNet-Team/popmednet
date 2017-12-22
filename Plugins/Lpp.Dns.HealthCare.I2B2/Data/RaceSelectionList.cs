using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.I2B2.Data
{
    public class RaceSelectionList
    {
        public readonly static RaceSelectionList Unknown = new RaceSelectionList("Unknown", 0);
        public readonly static RaceSelectionList Native = new RaceSelectionList("American Indian or Alaska Native", 1);
        public readonly static RaceSelectionList Asian = new RaceSelectionList("Asian", 2);
        public readonly static RaceSelectionList Black = new RaceSelectionList("Black or African American", 3);
        public readonly static RaceSelectionList Pacific = new RaceSelectionList("Native Hawaiian or Other Pacific Islander (NHOPI)", 4);
        public readonly static RaceSelectionList White = new RaceSelectionList("White", 5);
        public readonly static RaceSelectionList Hispanic = new RaceSelectionList("Hispanic", 6);

        public readonly static RaceSelectionList[] races = { Unknown, Native, Asian, Black, Pacific, White, Hispanic };

        public string Name { get; set; }
        public int Code { get; set; }

        public RaceSelectionList(string name, int code)
        {
            Name = name;
            Code = code;
        }

        public static string GetName(int code)
        {
            foreach (RaceSelectionList race in races)
            {
                if (race.Code == code)
                    return race.Name;
            }

            return null;
        }
    }
}
