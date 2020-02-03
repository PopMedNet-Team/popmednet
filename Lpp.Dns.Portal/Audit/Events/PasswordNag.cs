using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    [Guid("C2790001-2FF6-456C-9497-A22200CCCD1F")]
    [DisplayName( "Password Expiration Reminder" )]
    public class PasswordNag
    {
        [AudProp( CommonProperties.TargetUser )]
        public int TargetUser { get; set; }

        [AudProp( "{24220D92-B556-47AF-A23D-7236C7E51626}" )]
        public int DaysLeft { get; set; }
    }
}