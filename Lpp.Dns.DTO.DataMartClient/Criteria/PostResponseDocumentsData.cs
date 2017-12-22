using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.DataMartClient.Criteria
{
    [DataContract]
    public class PostResponseDocumentsData
    {
        [DataMember]
        public Guid RequestID { get; set; }

        [DataMember]
        public Guid DataMartID { get; set; }

        [DataMember]
        public IEnumerable<Document> Documents { get; set; }
    }
}
