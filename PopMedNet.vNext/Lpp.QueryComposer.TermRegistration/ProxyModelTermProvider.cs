using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.QueryComposer.TermRegistration
{
    [Serializable]
    public class ProxyModelTermProvider : MarshalByRefObject, IModelTermProvider
    {
        readonly IModelTermProvider provider;

        public ProxyModelTermProvider()
        {
            this.provider = new EmptyModelTermProvider();
        }

        public ProxyModelTermProvider(IModelTermProvider provider)
        {
            this.provider = provider;
        }

        public Guid ModelID
        {
            get { return this.provider.ModelID; }
        }

        public Guid ProcessorID
        {
            get { return this.provider.ProcessorID; }
        }

        public string Processor
        {
            get { return this.provider.Processor; }
        }

        public IEnumerable<IModelTerm> Terms
        {
            get { return this.provider.Terms; }
        }

        public sealed class EmptyModelTermProvider : IModelTermProvider
        {

            public Guid ModelID
            {
                get { return Guid.Empty; }
            }

            public Guid ProcessorID
            {
                get { return Guid.Empty; }
            }

            public string Processor
            {
                get { return string.Empty; }
            }

            public IEnumerable<IModelTerm> Terms
            {
                get { return new IModelTerm[0]; }
            }
        }
    }
}
