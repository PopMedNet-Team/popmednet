using System;
using System.Collections.Generic;

namespace Lpp.Dns.HealthCare.Summary
{
    public partial class StratificationCategoryLookUp
    {
        public readonly static StratificationCategoryLookUp Strat10 = new StratificationCategoryLookUp("age", 1, "10 Stratifications", "(0-1,2-4,5-9,10-14,15-18,19-21,22-44,45-64,65-74,75+)", "10,0-1,2-4,5-9,10-14,15-18,19-21,22-44,45-64,65-74,75-+");
        public readonly static StratificationCategoryLookUp Strat7 = new StratificationCategoryLookUp("age", 2, "7 Stratifications", "(0-4,5-9,10-18,19-21,22-44,45-64,65+)", "7,0-4,5-9,10-18,19-21,22-44,45-64,65-+");
        public readonly static StratificationCategoryLookUp Strat4 = new StratificationCategoryLookUp("age", 3, "4 Stratifications", "(0-21,22-44,45-64,65+)", "4,0-21,22-44,45-64,65-+");
        public readonly static StratificationCategoryLookUp Strat2 = new StratificationCategoryLookUp("age", 4, "2 Stratifications", "(Under 65,65+)", "2,0-64,65-+");
        public readonly static StratificationCategoryLookUp Strat0 = new StratificationCategoryLookUp("age", 5, "No Stratifications", "(0+)", "0,0-+");
        public readonly static StratificationCategoryLookUp FemaleOnly = new StratificationCategoryLookUp("sex", 1, "Female Only", "", "0,F-F");
        public readonly static StratificationCategoryLookUp MaleOnly = new StratificationCategoryLookUp("sex", 2, "Male Only", "", "0,M-M");
        public readonly static StratificationCategoryLookUp MaleAndFemale = new StratificationCategoryLookUp("sex", 3, "Male and Female", "", "0,F-M");
        public readonly static StratificationCategoryLookUp MaleFemaleAggr = new StratificationCategoryLookUp("sex", 3, "Male and Female Aggregated", "", "1,F-M");

        public readonly static StratificationCategoryLookUp[] All = { Strat10, Strat7, Strat4, Strat2, Strat0, FemaleOnly, MaleOnly, MaleAndFemale, MaleFemaleAggr };

        public string StratificationType { get; set; }
        public int StratificationCategoryId { get; set; }
        public string CategoryText { get; set; }
        public string ClassificationText { get; set; }
        public string ClassificationFormat { get; set; }

        public StratificationCategoryLookUp(string stratType, int catID, string catText, string classText, string classFormat)
        {
            this.StratificationCategoryId = catID;
            this.StratificationType = stratType;
            this.ClassificationText = classText;
            this.CategoryText = catText;
            this.ClassificationFormat = classFormat;
        }
    }
}
