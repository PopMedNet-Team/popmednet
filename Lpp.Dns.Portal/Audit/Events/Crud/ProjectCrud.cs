using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    [Guid("1C090001-9F95-400C-9780-A22200CD0234")]
    [DisplayName( "Project Change" )]
    public class ProjectCrud : CrudEvent
    {
        [AudProp( CommonProperties.Project )]
        public Guid Project { get; set; }
    }
}