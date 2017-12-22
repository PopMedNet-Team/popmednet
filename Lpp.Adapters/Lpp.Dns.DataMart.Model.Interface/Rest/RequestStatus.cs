using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DataMart.Model.Rest
{
    /// <summary>
    /// Corresponds to IModelProcessor's RequestStatus class.
    /// Encapsulate current status code and message if any after a Request is made.
    /// </summary>
    [DataContract(Namespace = "http://lincolnpeak.com/schemas/DNS4/API")]
    public class RequestStatus
    {
        [DataMember]
        public StatusCode Code { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}
