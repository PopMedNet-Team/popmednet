using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.DataMartClient
{
    [DataContract]
    public class Document
    {
        [DataMember]
        public long Size { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string MimeType { get; set; }
        [DataMember]
        public bool IsViewable { get; set; }
        [DataMember]
        public string Kind { get; set; }
    }

    [DataContract]
    public class DocumentWithID
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public Document Document { get; set; }
    }
}
