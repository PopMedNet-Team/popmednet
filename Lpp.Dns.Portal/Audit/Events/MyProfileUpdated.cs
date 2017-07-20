using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    [Guid("B6B10001-07FB-47F5-83B8-A22200CCDB90")]
    [DisplayName( "My Profile Updated" )]
    public class MyProfileUpdated
    {
        [AudProp( CommonProperties.TargetUser )]
        public int TargetUser { get; set; }

        [AudProp( CommonProperties.ActingUser )]
        public int ActingUser { get; set; }
    }
}