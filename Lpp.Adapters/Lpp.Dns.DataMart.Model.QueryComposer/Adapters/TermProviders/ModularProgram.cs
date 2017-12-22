using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.TermProviders
{
    [Serializable]
    public class ModularProgram : IModelTermProvider
    {
        public Guid ModelID
        {
            get { return new Guid("1B0FFD4C-3EEF-479D-A5C4-69D8BA0D0154"); }
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
