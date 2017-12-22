using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.DataMartClient
{
    [DataContract]
    public class Model
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public Guid ProcessorID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Properties { get; set; }
        [DataMember]
        public string PackageIdentifier { get; set; }
    }
}
