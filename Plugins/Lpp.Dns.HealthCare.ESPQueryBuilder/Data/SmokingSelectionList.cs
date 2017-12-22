using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder.Data
{
    public class SmokingSelectionList
    {
        
        public readonly static SmokingSelectionList Current = new SmokingSelectionList("Current", 1);
        public readonly static SmokingSelectionList Former = new SmokingSelectionList("Former", 2);
        public readonly static SmokingSelectionList Never = new SmokingSelectionList("Never", 3);
        public readonly static SmokingSelectionList NA = new SmokingSelectionList("Not Available", 4);
        public readonly static SmokingSelectionList Passive = new SmokingSelectionList("Passive", 5);

        public readonly static SmokingSelectionList[] smokings = { Current, Former, Never, NA, Passive };

        public string Name { get; set; }
        public int Code { get; set; }

        public SmokingSelectionList(string name, int code)
        {
            Name = name;
            Code = code;
        }

        public static string GetName(int code)
        {
            foreach (SmokingSelectionList smoking in smokings)
            {
                if (smoking.Code == code)
                    return smoking.Name;
            }

            return null;
        }
    }
}
