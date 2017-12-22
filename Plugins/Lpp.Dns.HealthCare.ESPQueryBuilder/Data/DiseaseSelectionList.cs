using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder.Data
{
    public class DiseaseSelectionList
    {
        // Diabetes: Prediabetes
        // Diabetes: Gestational diabetes
        // Diabetes: Type 1 diabetes
        // Diabetes: Type 2 diabetes
        // Influenza-like Illness
        public readonly static DiseaseSelectionList Influenza = new DiseaseSelectionList("ili", "Influenza-like Illness", 1);
        public readonly static DiseaseSelectionList DiabetesType1 = new DiseaseSelectionList("diabetes:type-1", "Diabetes: Type 1 diabetes", 2);
        public readonly static DiseaseSelectionList DiabetesType2 = new DiseaseSelectionList("diabetes:type-2", "Diabetes: Type 2 diabetes", 3);
        public readonly static DiseaseSelectionList DiabetesGestational = new DiseaseSelectionList("diabetes:gestational", "Diabetes: Gestational diabetes", 4);
        public readonly static DiseaseSelectionList DiabetesPrediabetes = new DiseaseSelectionList("diabetes:prediabetes", "Diabetes: Prediabetes", 5);
        public readonly static DiseaseSelectionList Asthma = new DiseaseSelectionList("asthma", "Asthma", 6);
        public readonly static DiseaseSelectionList Depression = new DiseaseSelectionList("dep", "Depression", 7);
        public readonly static DiseaseSelectionList OpioidPrescription = new DiseaseSelectionList("opioidrx", "Opioid Prescription", 8);
        public readonly static DiseaseSelectionList BenzodiazepinePrescription = new DiseaseSelectionList("benzodiarx", "Benzodiazepine Prescription", 9);
        public readonly static DiseaseSelectionList ConcurrentBenzodiazepineOpioidPrescription = new DiseaseSelectionList("benzopiconcurrent", "Concurrent Benzodiazepine-Opioid Prescription", 10);
        public readonly static DiseaseSelectionList HighOpioidUse = new DiseaseSelectionList("highopioiduse", "High Opioid Use", 11);

        public readonly static DiseaseSelectionList[] diseases = { Influenza, DiabetesType1, DiabetesType2, DiabetesGestational, DiabetesPrediabetes, Asthma, Depression, OpioidPrescription, BenzodiazepinePrescription, ConcurrentBenzodiazepineOpioidPrescription, HighOpioidUse};

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
