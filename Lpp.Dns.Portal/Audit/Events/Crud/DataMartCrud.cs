using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    [Guid("59A90001-539E-4C21-A4F2-A22200CD3C7D")]
    [DisplayName( "DataMart Change" )]
    public class DataMartCrud : CrudEvent
    {
        [AudProp( CommonProperties.DataMart )]
        public int DataMart { get; set; }

        [AudProp( CommonProperties.Organization )]
        public int Organization { get; set; }
    }
}