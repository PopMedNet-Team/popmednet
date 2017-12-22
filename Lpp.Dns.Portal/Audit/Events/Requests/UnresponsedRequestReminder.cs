using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal.Events
{
    [Guid("688B0001-1572-41CA-8298-A22200CBD542")]
    [DisplayName( "Submitted Request Awaits a Response" )]
    public class UnrespondedRequestReminder : RoutingEventBase { public UnrespondedRequestReminder( RequestDataMart r ) : base( r ) { } }
}