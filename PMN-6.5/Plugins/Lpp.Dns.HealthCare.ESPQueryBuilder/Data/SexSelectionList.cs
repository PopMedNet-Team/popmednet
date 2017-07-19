using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder.Data
{
    public class SexSelectionList
    {
        public readonly static SexSelectionList Male = new SexSelectionList("Male", (int)SexSelectionCode.Male);
        public readonly static SexSelectionList Female = new SexSelectionList("Female", (int)SexSelectionCode.Female);
        public readonly static SexSelectionList Both = new SexSelectionList("Male and Female", (int)SexSelectionCode.Both);

        public readonly static SexSelectionList[] sexes = { Male, Female, Both };

        public string Name { get; set; }
        public int Code { get; set; }

        public SexSelectionList(string name, int code)
        {
            Name = name;
            Code = code;
        }

        public static string GetName(int code)
        {
            foreach (SexSelectionList sex in sexes)
            {
                if (sex.Code == code)
                    return sex.Name;
            }

            return null;
        }
        
    }

    public enum SexSelectionCode
    {
        Male = 1,
        Female = 2,
        Both = 3
    }
}
