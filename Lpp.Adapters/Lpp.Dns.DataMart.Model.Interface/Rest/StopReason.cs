using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DataMart.Model.Rest
{
    /// <summary>
    /// Corresponds to IModelProcessor's StopReason.
    /// Expresses the reason for calling Stop.
    /// </summary>
    [DataContract(Namespace = "http://lincolnpeak.com/schemas/DNS4/API")]
    public enum StopReason
    {
        [EnumMember]
        Shutdown = 0,
        [EnumMember]
        Cancel = 1
    }
}
