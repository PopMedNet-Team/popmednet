using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Views.ESPQueryBuilder.Terms;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder.Code
{
    //public class ESPTerm<TView> where TView : WebViewPage
    public class Term
    {
        public static IEnumerable<Term> All { get { return AllTerms; } }

        public static readonly IEnumerable<Term> AllTerms = new[]
        {
            new Term( "ICD9CodeSelector", "Diagnosis" ),
            new Term( "DiseaseSelector", "Condition" ),
            new Term( "Visits", "Visits" ),
            new Term( "Dummy", "Demographic", new[] { new Term( "AgeRange", "Age Range" ),
                                                      new Term( "Gender", "Sex" ),
                                                      new Term( "RaceSelector", "Race" ),
                                                      new Term( "EthnicitySelector", "Race-Ethnicity" ),
                                                      new Term( "SmokingSelector", "Tobacco Use"),
                                                      new Term( "ZipCodeSelector", "Zip Codes" ),
                                                      new Term("PredefinedLocation", "Location"),
                                                      new Term("CustomLocation", "Custom Location")
            } ),

        };

        public string Name { get; set; }
        public string Label { get; set; }
        public IEnumerable<Term> Terms { get; set; }

        public Term(string name, string label, IEnumerable<Term> terms = null)
        {
            Name = name;
            Label = label;
            Terms = terms;
        }

    }
}
