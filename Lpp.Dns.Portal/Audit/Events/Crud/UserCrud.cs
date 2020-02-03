using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    [Guid("B7640001-7247-49B8-A818-A22200CCEAF7")]
    [DisplayName( "User Change" )]
    public class UserCrud : CrudEvent
    {
        [AudProp( CommonProperties.TargetUser )]
        public int TargetUser { get; set; }

        [AudProp( CommonProperties.Organization )]
        public int Organization { get; set; }
    }
}