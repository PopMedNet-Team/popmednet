using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    [Guid("B8A50001-B556-43D2-A1B8-A22200CD12DC")]
    [DisplayName( "Organization Change" )]
    public class OrganizationCrud : CrudEvent
    {
        [AudProp( CommonProperties.Organization )]
        public int Organization { get; set; }
    }
}