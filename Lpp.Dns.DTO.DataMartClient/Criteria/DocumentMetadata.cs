using System;
using System.Runtime.Serialization;

namespace Lpp.Dns.DTO.DataMartClient.Criteria
{
    [DataContract]
    public class DocumentMetadata
    {
        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public Guid RequestID { get; set; }

        [DataMember]
        public Guid DataMartID { get; set; }

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

        [DataMember]
        public int CurrentChunkIndex { get; set; }
    }
}
