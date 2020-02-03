using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.TermProviders
{
    [Serializable]
    public class QueryComposer : IModelTermProvider
    {
        public Guid ModelID
        {
            get { return new Guid("455C772A-DF9B-4C6B-A6B0-D4FD4DD98488"); }
        }

        public Guid ProcessorID
        {
            get { return new Guid("AE0DA7B0-0F73-4D06-B70B-922032B7F0EB"); }
        }

        public string Processor
        {
            get { return "Query Composer Model Processor"; }
        }

        public IEnumerable<IModelTerm> Terms
        {
            get { return ModelTermsFactory.Terms; }
        }
    }
}
