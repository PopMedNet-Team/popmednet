using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal.Events
{
    [Guid("B7B30001-2704-4A57-A71A-A22200CC1736")]
    [DisplayName( "Submitted Request Needs Approval" )]
    public class UnapprovedRequestReminder : RequestEventBase { public UnapprovedRequestReminder( Request r ) : base( r ) { } }
}