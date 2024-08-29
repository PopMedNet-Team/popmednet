using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    [Guid("553FD350-8F3B-40C6-9E31-11D8BC7420A2")]
    [DisplayName("Registry Change")]
    public class RegistryCrud : CrudEvent
    {
        [AudProp( CommonProperties.Registry)]
        public Guid Registry { get; set; }
    }
}