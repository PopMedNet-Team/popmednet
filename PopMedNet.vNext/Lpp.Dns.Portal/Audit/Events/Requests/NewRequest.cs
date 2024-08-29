using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    [Guid("06E30001-ED86-4427-9936-A22200CC74F0")]
    [DisplayName( "New Request Submitted" )]
    public class NewRequest
    {
        [AudProp( CommonProperties.ActingUser )]
        public int ActingUser { get; set; }

        [AudProp( CommonProperties.Project )]
        public Guid Project { get; set; }

        [AudProp( CommonProperties.Request )]
        public int Request { get; set; }

        [AudProp( CommonProperties.RequestType )]
        public Guid RequestType { get; set; }

        [AudProp( CommonProperties.Name )]
        public string Name { get; set; }
    }
}