using System;
using System.Collections.Generic;
using Lpp.Security;
using Lpp.Dns.Data;

namespace Lpp.Dns
{
    public interface IDnsRequestContext
    {
        Guid RequestID { get; }

        //TODO: get rid of this function or at least change securityprivilege to Guid for the permission id
        Func<SecurityPrivilege, bool> Can { get; }

        DnsRequestHeader Header { get; }

        IDnsModel Model { get; }

        IDnsRequestType RequestType { get; }

        IEnumerable<Document> Documents { get; }

        IEnumerable<IDnsDataMart> DataMarts { get; }

        IEnumerable<IDnsActivity> Activities { get; }

        void ModifyMetadata( DnsRequestMetadata md );
    }    
}