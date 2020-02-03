using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.TermProviders
{
    [Serializable]
    public class DistributedRegression : IModelTermProvider
    {
        public Guid ModelID
        {
            get { return new Guid("4C8A25DC-6816-4202-88F4-6D17E72A43BC"); }
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
            get { return ModelTermsFactory.Terms.Where(t => t.ID == ModelTermsFactory.ModularProgramID || t.ID == ModelTermsFactory.FileUploadID).ToArray(); }
        }
    }
}
