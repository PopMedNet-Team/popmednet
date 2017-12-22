using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    [Guid("25EC0001-3AC0-45FB-AF72-A22200CC334C")]
    [DisplayName( "Results Viewed" )]
    public class ResultsViewed
    {
        [AudProp( CommonProperties.ActingUser )]
        public Guid ActingUser { get; set; }

        [AudProp( CommonProperties.Request )]
        public Guid Request { get; set; }
    }
}