using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.Summary.Data
{
    public class CoverageSelectionList
    {
        public const string NACode = "NA";
        public const string DrugAndMedicalCode = "DRUG|MED";
        public const string DrugOnlyCode = "DRUG";
        public const string MedicalOnlyCode = "MED";
        public const string AllMembersCode = "ALL";

        public readonly static CoverageSelectionList NA = new CoverageSelectionList("Not Applicable", NACode);
        public readonly static CoverageSelectionList DrugAndMedical = new CoverageSelectionList("Drug and Medical Coverage", DrugAndMedicalCode);
        public readonly static CoverageSelectionList DrugOnly = new CoverageSelectionList("Drug Coverage Only", DrugOnlyCode);
        public readonly static CoverageSelectionList MedicalOnly = new CoverageSelectionList("Medical Coverage Only", MedicalOnlyCode);
        public readonly static CoverageSelectionList AllMembers = new CoverageSelectionList("All Members", AllMembersCode);

        public readonly static CoverageSelectionList[] Coverages = { NA, DrugAndMedical, DrugOnly, MedicalOnly, AllMembers };

        public string Name { get; set; }
        public string Code { get; set; }

        public CoverageSelectionList(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public static string GetName(string code)
        {
            foreach (CoverageSelectionList Coverage in Coverages)
            {
                if (Coverage.Code == code)
                    return Coverage.Name;
            }

            return null;
        }
    }

    public partial class CoverageSelectionLookUp
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
