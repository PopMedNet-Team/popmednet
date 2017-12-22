using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.I2B2.Data
{
    public class DiseaseSelectionList
    {
        public readonly static DiseaseSelectionList Influenza = new DiseaseSelectionList("Influenza", "Influenza-like Illness", 1);
        public readonly static DiseaseSelectionList Diabetes = new DiseaseSelectionList("Diabetes", "Diabetes", 2);

        public readonly static DiseaseSelectionList[] diseases = { Influenza, Diabetes };

        public string Name { get; set; }
        public string Display { get; set; }
        public int Code { get; set; }

        public DiseaseSelectionList(string name, string display, int code)
        {
            Name = name;
            Display = display;
            Code = code;
        }

        public static string GetName(int code)
        {
            foreach (DiseaseSelectionList disease in diseases)
            {
                if (disease.Code == code)
                    return disease.Name;
            }

            return null;
        }
        
    }
}
