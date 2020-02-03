using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    [Guid("D2460001-F0FA-4BAA-AEE1-A22200CCADB4")]
    [DisplayName( "Project Assignment" )]
    public class ProjectAssignment
    {
        [AudProp( CommonProperties.TargetUser )]
        public int AssignedUser { get; set; }

        [AudProp( CommonProperties.Project )]
        public Guid ProjectId { get; set; }

    }
}