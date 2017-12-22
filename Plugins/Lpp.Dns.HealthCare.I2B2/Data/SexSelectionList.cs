using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.I2B2.Data
{
    public class SexSelectionList
    {
        public readonly static SexSelectionList Male = new SexSelectionList("Male", 1);
        public readonly static SexSelectionList Female = new SexSelectionList("Female", 2);
        public readonly static SexSelectionList Both = new SexSelectionList("Male and Female", 3);

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
}
