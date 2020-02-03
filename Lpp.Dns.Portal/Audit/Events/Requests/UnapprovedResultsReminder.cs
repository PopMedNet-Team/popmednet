using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal.Events
{
    [Guid("F31C0001-6900-4BDB-A03A-A22200CC019C")]
    [DisplayName( "Uploaded Result Needs Approval" )]
    public class UnapprovedResultsReminder : RoutingEventBase 
    { 
        public UnapprovedResultsReminder( RequestDataMart r ) : base( r ) 
        { 
        } 
    }
}