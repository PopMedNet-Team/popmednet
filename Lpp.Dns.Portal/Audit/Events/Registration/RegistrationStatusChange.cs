using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    [Guid("76B10001-2B49-453C-A8E1-A22200CC9356")]
    [DisplayName( "Registration Status Change" )]
    public class RegistrationStatusChange
    {
        [AudProp( CommonProperties.TargetUser )]
        public int SignUpUser { get; set; }

        [AudProp( CommonProperties.Organization )]
        public int Organization { get; set; }

        [AudProp("{E6300A74-A278-4F80-B978-D640751AF32C}")]
        public string OldStatus { get; set; }

        [AudProp("{EA136ACB-CCB8-4574-A7BC-9FDEE1F3D4DA}")]
        public string NewStatus { get; set; }
    }
}