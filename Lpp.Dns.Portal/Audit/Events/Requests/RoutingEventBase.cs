using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal.Events
{
    public abstract class RoutingEventBase : RequestEventBase
    {
        [AudProp( CommonProperties.DataMart )]
        public Guid DataMart { get; set; }

        public RoutingEventBase( RequestDataMart r ) : base( r.Request )
        {
            this.DataMart = r.DataMartID;
        }
    }
}