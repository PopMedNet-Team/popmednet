using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.DataMartClient
{
    [DataContract]
    public class RequestTypeIdentifier
    {
        [DataMember]
        public string Identifier { get; set; }
        [DataMember]
        public string Version { get; set; }

        public string PackageName()
        {
            return string.Format("{0}.{1}.zip", Identifier, Version);
        }
    }
}
