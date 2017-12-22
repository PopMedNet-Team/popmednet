using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    [Guid("3AC20001-D8A4-4BE7-957C-A22200CC84BB")]
    [DisplayName( "Registration Submitted" )]
    public class RegistrationSubmitted
    {
        [AudProp( CommonProperties.TargetUser )]
        public int SignUpUser { get; set; }

        [AudProp( CommonProperties.Organization )]
        public int Organization { get; set; }

    }
}