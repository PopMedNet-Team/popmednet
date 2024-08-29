using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DataMart.Model.Rest
{
    /// <summary>
    /// Corresponds to IModelProcessor's Document class.
    /// Provides information about Doucment to be transferred.
    /// </summary>
    [DataContract(Namespace = "http://lincolnpeak.com/schemas/DNS4/API")]
    public class Document
    {
        [DataMember]
        public string DocumentId { get; set; }
        [DataMember]
        public string MimeType { get; set; }
        [DataMember]
        public int Size { get; set; }
        [DataMember]
        public bool Viewable { get; set; }
        [DataMember]
        public string Filename { get; set; }
    }
}
