using LinqKit;
using Lpp.Dns.DTO.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters
{
    public static class AdapterHelpers
    {
        /// <summary>
        /// Parses the concept values for the Age Range concept.
        /// </summary>
        /// <param name="pTerm"></param>
        /// <returns></returns>
        public static AgeRangeValues ParseAgeRangeValues(QueryComposerTermDTO pTerm)
        {
            if (pTerm == null)
                return null;

            int? minAge = null;
            int? maxAge = null;
            DateTimeOffset? birthStartDate = null;
            DateTimeOffset? birthEndDate = null;
            DTO.Enums.AgeRangeCalculationType? calculationType = null;
            DateTime? calculateAsOf = null;

            if (pTerm.GetValue("MinAge") != null)
            {
                int tempValue;
                if (Int32.TryParse(pTerm.GetStringValue("MinAge"), out tempValue))
                {
                    minAge = tempValue;
                }
            }

            if (pTerm.GetValue("MaxAge") != null)
            {
                int tempValue;
                if (Int32.TryParse(pTerm.GetStringValue("MaxAge"), out tempValue))
                {
                    maxAge = tempValue;
                }
            }
            if (!string.IsNullOrWhiteSpace(pTerm.GetStringValue("BirthStartDate")))
            {
                DateTimeOffset tempDate;
                if (DateTimeOffset.TryParse(pTerm.GetStringValue("BirthStartDate"), out tempDate))
                {
                    birthStartDate = tempDate;
                }
            }
            if (!string.IsNullOrWhiteSpace(pTerm.GetStringValue("BirthEndDate")))
            {
                DateTimeOffset tempDate;
                if (DateTimeOffset.TryParse(pTerm.GetStringValue("BirthEndDate"), out tempDate))
                {
                    birthEndDate = tempDate;
                }
            }

            DTO.Enums.AgeRangeCalculationType calculateTypeAttempt;
            if (pTerm.GetEnumValue<DTO.Enums.AgeRangeCalculationType>("CalculationType", out calculateTypeAttempt))
            {
                calculationType = calculateTypeAttempt;
            }

            if (!string.IsNullOrWhiteSpace("CalculateAsOf"))
            {
                DateTime tempDate;
                if (DateTime.TryParse(pTerm.GetStringValue("CalculateAsOf"), out tempDate))
                {
                    calculateAsOf = tempDate;
                }
            }

            return new AgeRangeValues { MinAge = minAge, MaxAge = maxAge, BirthStartDate = birthStartDate, BirthEndDate = birthEndDate, CalculateAsOf = calculateAsOf, CalculationType = calculationType };
        }

        /// <summary>
        /// For the specified terms converts the AgeRange terms that have the specified calculationType to AgeRangeValues.
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="calculationType"></param>
        /// <returns></returns>
        public static IEnumerable<AgeRangeValues> ParseAgeRangeValues(IEnumerable<QueryComposerTermDTO> terms, IEnumerable<DTO.Enums.AgeRangeCalculationType> calculationTypes)
        {
            var ranges = terms.Where(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.AgeRangeID)
                             .Select(t => ParseAgeRangeValues(t))
                             .Where(r => r != null && r.CalculationType.HasValue && (calculationTypes == null || calculationTypes.Count() == 0 || calculationTypes.Contains(r.CalculationType.Value)));

            return ranges;
        }


        public static HeightValues ParseHeightValues(QueryComposerTermDTO pTerm)
        {

            var range = new HeightValues();

            double value;
            if (double.TryParse(pTerm.GetStringValue("HeightMin"), out value))
            {
                range.MinHeight = value;
            }

            if (double.TryParse(pTerm.GetStringValue("HeightMax"), out value))
            {
                range.MaxHeight = value;
            }

            return range;
        }

        public static WeightValues ParseWeightValues(QueryComposerTermDTO pTerm)
        {
            var range = new WeightValues();

            double value;
            if (double.TryParse(pTerm.GetStringValue("WeightMin"), out value))
            {
                range.MinWeight = value;
            }

            if (double.TryParse(pTerm.GetStringValue("WeightMax"), out value))
            {
                range.MaxWeight = value;
            }

            return range;
        }
        public static IEnumerable<string> ParseCodeTermValues(QueryComposerTermDTO pTerm)
        {
            IEnumerable<string> codeTermValues = from v in pTerm.GetCodeStringCollection()
                                                 where !string.IsNullOrWhiteSpace(v)
                                                 select v.Trim();
            IEnumerable<string> codes = codeTermValues.Distinct();
            return codes;
        }

        public static IEnumerable<string> ParseOtherCodeTermValues(QueryComposerTermDTO pTerm)
        {
            string[] codes = pTerm.GetStringValue("OtherCode").Split(',').Distinct().ToArray();

            return codes;
        }

        public static DateRangeValues ParseDateRangeValues(QueryComposerTermDTO pTerm)
        {
            DateRangeValues val = new DateRangeValues();

            DateTimeOffset date;
            if (DateTimeOffset.TryParse((pTerm.GetStringValue("Start") ?? string.Empty).ToString(), out date))
            {
                val.StartDate = date;
            }
            if (DateTimeOffset.TryParse((pTerm.GetStringValue("End") ?? string.Empty).ToString(), out date))
            {
                val.EndDate = date;
            }

            if (DateTimeOffset.TryParse((pTerm.GetStringValue("StartDate") ?? string.Empty).ToString(), out date))
            {
                val.StartDate = date;
            }
            if (DateTimeOffset.TryParse((pTerm.GetStringValue("EndDate") ?? string.Empty).ToString(), out date))
            {
                val.EndDate = date;
            }

            return val;
        }

        public static EncounterPeriodValues ParseEncounterPeriodValues(QueryComposerTermDTO pTerm)
        {
            EncounterPeriodValues val = new EncounterPeriodValues();

            DateTimeOffset date;

            if (DateTimeOffset.TryParse((pTerm.GetStringValue("AdmissionDate") ?? string.Empty).ToString(), out date))
            {
                val.AdmissionDate = date;
            }
            if (DateTimeOffset.TryParse((pTerm.GetStringValue("DischargeDate") ?? string.Empty).ToString(), out date))
            {
                val.DischargeDate = date;
            }

            return val;
        }

        /// <summary>
        /// Terms that need to be grouped in sub-criteria and multiple terms OR'd together.
        /// </summary>
        public static readonly Guid[] GroupedTerms = new[]{
            //Condition
	        new Guid("EC593176-D0BF-4E5A-BCFF-4BBD13E88DBF"),
	        //HCPCS Procedure Codes
	        new Guid("096A0001-73B4-405D-B45F-A3CA014C6E7D"),
	        //Combined Diagnosis Codes
	        new Guid("86110001-4BAB-4183-B0EA-A4BC0125A6A7"),
	        //ICD9 Diagnosis Codes 3 digit
	        new Guid("5E5020DC-C0E4-487F-ADF2-45431C2B7695"),
	        //ICD9 Diagnosis Codes 4 digit
	        new Guid("D0800001-2810-48ED-96B9-A3D40146BAAE"),
	        //ICD9 Diagnosis Codes 5 digit
	        new Guid("80750001-6C3B-4C2D-90EC-A3D40146C26D"),
	        //ICD9 Procedure Codes 3 digit
	        new Guid("E1CC0001-1D9A-442A-94C4-A3CA014C7B94"),
	        //ICD9 Procedure Codes 4 digit
	        new Guid("9E870001-1D48-4AA3-8889-A3D40146CCB3"),
	        //Visits
	        new Guid("F01BE0A4-7D8E-4288-AE33-C65166AF8335"),
	        //Drug Class
	        new Guid("75290001-0E78-490C-9635-A3CA01550704"),
	        //Drug Name
	        new Guid("0E1F0001-CA0C-42D2-A9CC-A3CA01550E84"),
	        //Age Range
	        new Guid("D9DD6E82-BBCA-466A-8022-B54FF3D99A3C"),
	        //Sex
	        new Guid("71B4545C-345B-48B2-AF5E-F84DC18E4E1A")
        };

        /// <summary>
        /// Determines if the specified term will be in a sub-criteria.
        /// </summary>
        /// <param name="termID">The ID of the term.</param>
        /// <returns></returns>
        public static bool IsGroupedTerm(Guid termID)
        {
            return GroupedTerms.Contains(termID);
        }

        /// <summary>
        /// Non-code terms that should be grouped in a sub-criteria with each term OR'd, the sub-criteria for these terms are homogenous.
        /// All code terms are grouped in a single sub-criteria.
        /// </summary>
        public static readonly Guid[] NonCodeGroupedTerms = new[] {
            //Visits
	        new Guid("F01BE0A4-7D8E-4288-AE33-C65166AF8335"),
            //Age Range
	        new Guid("D9DD6E82-BBCA-466A-8022-B54FF3D99A3C"),
	        //Sex
	        new Guid("71B4545C-345B-48B2-AF5E-F84DC18E4E1A")
        };

        /// <summary>
        /// Determines if the specified term will be in a specific sub-criteria containing only the specified non-code term type.
        /// </summary>
        /// <param name="termID">The ID of the term.</param>
        /// <returns></returns>
        public static bool IsNonCodeTerm(Guid termID)
        {
            return NonCodeGroupedTerms.Contains(termID);
        }
    }

    public enum Race
    {
        Native = 1,
        Asian = 2,
        Black = 3,
        Pacific = 4,
        White = 5,
        Unknown = 6
    }

    public enum Ethnicities
    {
        Hispanic = 0,
        NotHispanic = 1,
        Unknown = 2
    }

    public enum PCORIEthnicities
    {
        Hispanic = 0,
        NotHispanic = 1,
        Refuse = 2,
        Unknown = 3,        
        Other = 4
    }

    public enum PCORIRaces
    {        
        Native = 1,
        Asian = 2,
        Black = 3,
        Pacific = 4,
        White = 5,
        Multi = 6,
        Refuse = 7,
        NoInfo = 8,
        Unknown = 9,
        Other = 10
    }
}
