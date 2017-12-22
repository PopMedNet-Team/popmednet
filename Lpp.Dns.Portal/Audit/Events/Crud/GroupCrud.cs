using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    [Guid("D80E0001-27BC-4FCB-BA75-A22200CD2426")]
    [DisplayName( "Group Change" )]
    public class GroupCrud : CrudEvent
    {
        [AudProp( CommonProperties.Group )]
        public int Group { get; set; }
    }
}