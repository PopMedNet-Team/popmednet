using System;
using System.ComponentModel.Composition;
using Lpp.Composition;
using Lpp.Dns.Model;
using Lpp.Security;

namespace Lpp.Dns.Portal
{
    [Export( typeof( ISecurityObjectProvider<DnsDomain> ) ), PartMetadata( ExportScope.Key, TransactionScope.Id )]
    class RequestTypeSecObjectProvider : ISecurityObjectProvider<DnsDomain>
    {
        [Import] public IPluginService Plugins { get; set; }
        
        public ISecurityObject Find( Guid id )
        {
            var rt = Plugins.GetPluginRequestType( id );
            return rt == null ? null : rt.RequestType.AsSecurityObject();
        }

        public SecurityObjectKind Kind
        {
            get { return Dns.RequestTypeSecObjectKind; }
        }
    }
}