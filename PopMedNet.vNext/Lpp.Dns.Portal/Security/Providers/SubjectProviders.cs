using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Lpp.Composition;
using Lpp.Composition.Modules;
//using Lpp.Data;
using Lpp.Dns.Data;
using Lpp.Security;

namespace Lpp.Dns.Portal
{
    // TODO: This is a temporary dirty hack to get around this problem: http://stackoverflow.com/questions/13520227
    // Once the problem is resolved, we'll use ExportMany instead.
    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    class SubjectProviders
    {
        [Import]
        internal ICompositionService Comp { get; set; }

        [Export]
        public ISecuritySubjectProvider<Lpp.Dns.Model.DnsDomain> Users { get { return e<User>(); } }
        [Export]
        public ISecuritySubjectProvider<Lpp.Dns.Model.DnsDomain> OGroups 
        {
            get 
            {
                return null;
                //return e<OrganizationSecurityGroup>();
            }
        }

        [Export]
        public ISecuritySubjectProvider<Lpp.Dns.Model.DnsDomain> PGroups 
        {
            get 
            {
                return null;
                //return e<ProjectSecurityGroup>(); 
            }
        }

        ISecuritySubjectProvider<Lpp.Dns.Model.DnsDomain> e<T>() where T : class, ISecuritySubject
        {
            return Comp.Compose(new RepositorySecSubjectProvider<Lpp.Dns.Model.DnsDomain, T>());
        }
    }
}