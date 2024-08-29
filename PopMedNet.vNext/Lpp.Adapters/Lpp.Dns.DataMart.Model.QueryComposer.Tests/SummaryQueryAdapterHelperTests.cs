using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery;
using Lpp.Dns.DTO.QueryComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Tests
{
    [TestClass]
    public class SummaryQueryAdapterHelperTests
    {
        [TestMethod]
        public void ExpandYearsAndQuarters()
        {

            var adapter = new TestSummaryAdapter(new Dictionary<string, object>());

            var result = adapter.ExecuteExpandYearsWithQuarters(2000, 1, 2000, 4);

            Console.WriteLine(string.Join(",", result.ToArray()));
        }

    }

    public class TestSummaryAdapter : QueryComposer.Adapters.SummaryQuery.QueryAdapter
    {

        public TestSummaryAdapter(IDictionary<string, object> settings) : base(settings) { }

        public IEnumerable<string> ExecuteExpandYearsWithQuarters(int startYear, int startQuarter, int endYear, int endQuarter)
        {
            return ExpandYearsWithQuarters(startYear, startQuarter, endYear, endQuarter);
        }

        protected override bool IsMFU
        {
            get
            {
                return false;
            }
        }

        protected override string Template
        {
            get
            {
                return string.Empty;
            }
        }

        public override void Dispose()
        {
            
        }

        protected override SummaryRequestModel ConvertToModel(QueryComposerQueryDTO query)
        {
            throw new NotImplementedException();
        }

        protected override void ReplaceParameters(ref string query)
        {
            throw new NotImplementedException();
        }
    }
}

