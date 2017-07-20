using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Events
{
    public class RequestStatusChangeBase
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

        [AudProp( "{7BE6FDE5-3F2A-47C6-AC4C-CF171BD951FB}" )]
        public string OldStatus { get; set; }

        [AudProp( "{658E6DFD-957A-4745-BA65-48D862951BA6}" )]
        public string NewStatus { get; set; }

        [AudProp( CommonProperties.DataMart )]
        public int DataMart { get; set; }
    }

    [Guid("0A850001-FC8A-4DE2-9AA5-A22200E82398")]
    [DisplayName( "Request Status Changed" )]
    public class RequestStatusChange : RequestStatusChangeBase { }
}