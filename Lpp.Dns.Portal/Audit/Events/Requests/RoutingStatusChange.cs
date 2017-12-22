using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    [Guid("5AB90001-8072-42CD-940F-A22200CC24A2")]
    [DisplayName( "Routing Status Changed" )]
    public class RoutingStatusChange : RequestStatusChangeBase { }
}