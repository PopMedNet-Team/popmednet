using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.Conditions.Data
{
    public class DiseaseSelectionList
    {
        // Heart
        public readonly static DiseaseSelectionList coronary_art = new DiseaseSelectionList("coronary_art", "Coronary Artery Disease", 1);
        public readonly static DiseaseSelectionList atrial_fib = new DiseaseSelectionList("atrial_fib", "Atrial Fibrillation", 2);
        public readonly static DiseaseSelectionList heart_attack = new DiseaseSelectionList("heart_attack", "Heart attack or angina", 3);
        public readonly static DiseaseSelectionList congestive_heart = new DiseaseSelectionList("congestive_heart", "Congestive heart failure", 4);
        public readonly static DiseaseSelectionList icd = new DiseaseSelectionList("idc", "ICD or pacemaker placement", 5);

        // Cancer
        public readonly static DiseaseSelectionList breast_cancer = new DiseaseSelectionList("breast_cancer", "Breast cancer", 6);
        public readonly static DiseaseSelectionList colon_cancer = new DiseaseSelectionList("colon_cancer", "Colon cancer", 7);
        public readonly static DiseaseSelectionList lung_cancer = new DiseaseSelectionList("lung_cancer", "Lung cancer", 8);
        public readonly static DiseaseSelectionList prostate_cancer = new DiseaseSelectionList("prostate_cancer", "Prostate Cancer", 9);
        public readonly static DiseaseSelectionList cervical_cancer = new DiseaseSelectionList("cervical_cancer", "Cervical Cancer", 10);
        public readonly static DiseaseSelectionList melanoma = new DiseaseSelectionList("melanoma", "Melanoma", 11);
        public readonly static DiseaseSelectionList skin_cancer = new DiseaseSelectionList("skin_cancer", "Skin cancer, not melanoma", 12);
        public readonly static DiseaseSelectionList oral_cancer = new DiseaseSelectionList("oral_cancer", "Oral Cancer", 13);
        public readonly static DiseaseSelectionList other_cancer = new DiseaseSelectionList("other_cancer", "Other type of cancer", 14);

        // Metabolic
        public readonly static DiseaseSelectionList diabetes = new DiseaseSelectionList("diabetes", "Diabetes", 15);
        public readonly static DiseaseSelectionList high_cholesterol = new DiseaseSelectionList("high_cholesterol", "High cholesterol", 16);
        public readonly static DiseaseSelectionList thyroid_disease = new DiseaseSelectionList("thyroid_disease", "Thyroid disease", 17);
        public readonly static DiseaseSelectionList high_blood_pressure = new DiseaseSelectionList("high_blood_pressure", "High blood pressure", 18);
        public readonly static DiseaseSelectionList obesity = new DiseaseSelectionList("obesity", "Obesity", 19);

        // Lung/Respiratory
        public readonly static DiseaseSelectionList asthma = new DiseaseSelectionList("asthma", "Asthma", 20);
        public readonly static DiseaseSelectionList emphysema = new DiseaseSelectionList("emphysema", "Emphysema or 'COPD'", 21);

        // Bone/Joint
        public readonly static DiseaseSelectionList osteoarthritis = new DiseaseSelectionList("osteoarthritis", "Osteoarthritis", 22);
        public readonly static DiseaseSelectionList rheumatoid = new DiseaseSelectionList("rheumatoid", "Rheumatoid arthritis", 23);
        public readonly static DiseaseSelectionList other_autoimmune = new DiseaseSelectionList("other_autoimmune", "Other autoimmune disease", 24);
        public readonly static DiseaseSelectionList osteoporosis = new DiseaseSelectionList("osteoporosis", "Osteoporosis/Osteopenia", 25);
        public readonly static DiseaseSelectionList gout = new DiseaseSelectionList("gout", "Gout", 26);

        // Neurological
        public readonly static DiseaseSelectionList alzheimer = new DiseaseSelectionList("alzheimer", "Alzheimer’s disease", 27);
        public readonly static DiseaseSelectionList depression = new DiseaseSelectionList("depression", "Depression", 28);
        public readonly static DiseaseSelectionList other_mental = new DiseaseSelectionList("other_mental", "Other mental illness", 29);
        public readonly static DiseaseSelectionList stroke = new DiseaseSelectionList("stroke", "Stroke", 30);
        public readonly static DiseaseSelectionList multiple_sclerosis = new DiseaseSelectionList("multiple_sclerosis", "Multiple sclerosis", 31);

        // Gastrointestinal/Renal
        public readonly static DiseaseSelectionList crohn = new DiseaseSelectionList("crohn", "Crohn’s disease/ulcerative colitis", 32);
        public readonly static DiseaseSelectionList liver = new DiseaseSelectionList("liver", "Liver disease", 33);
        public readonly static DiseaseSelectionList kidney = new DiseaseSelectionList("kidney", "Kidney disease", 34);

        public readonly static DiseaseSelectionList[] diseases = { coronary_art, atrial_fib, heart_attack, congestive_heart, icd,
                                                                     breast_cancer, colon_cancer, lung_cancer, prostate_cancer, cervical_cancer,
                                                                     melanoma, skin_cancer, oral_cancer, other_cancer,
                                                                     diabetes, high_cholesterol, thyroid_disease, high_blood_pressure, obesity,
                                                                     asthma, emphysema, osteoarthritis, rheumatoid, other_autoimmune, osteoporosis,
                                                                   gout, alzheimer, depression, other_mental, stroke, multiple_sclerosis, crohn, 
                                                                   liver, kidney };

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
