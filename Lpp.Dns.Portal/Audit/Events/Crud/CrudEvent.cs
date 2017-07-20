using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    public abstract class CrudEvent
    {
        [AudProp( CommonProperties.ActingUser )]
        public int ActingUser { get; set; }

        [AudProp( CommonProperties.CrudEventKind )]
        public int CrudEventKind { get; set; }
    }

    public enum CrudEventKind { Added, Removed, Changed }
}