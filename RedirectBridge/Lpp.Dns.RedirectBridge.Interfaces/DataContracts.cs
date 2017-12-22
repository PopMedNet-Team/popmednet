using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Lpp.Dns
{
    [DataContract(Namespace = "http://lincolnpeak.com/schemas/DNS4/API")]
    [XmlRoot(Namespace = "http://lincolnpeak.com/schemas/DNS4/API")]
    public struct SessionMetadata
    {
        [DataMember]
        public Guid RequestID { get; set; }
        [DataMember]
        public string ReturnUrl { get; set; }
        [DataMember]
        public string ModelID { get; set; }
        [DataMember]
        public string RequestTypeID { get; set; }
        [DataMember]
        public UserIdentity User { get; set; }
        [DataMember]
        public string[] Activities { get; set; }
    }

    [DataContract(Namespace = "http://lincolnpeak.com/schemas/DNS4/API")]
    public struct UserIdentity
    {
        [DataMember]
        public string Username { get; set; }
    }

    [DataContract(Namespace = "http://lincolnpeak.com/schemas/DNS4/API")]
    public struct InvalidSessionFault
    {
        public static readonly InvalidSessionFault Instance = new InvalidSessionFault();
    }

    [DataContract(Namespace = "http://lincolnpeak.com/schemas/DNS4/API")]
    public struct DataMart
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Document[] Metadata { get; set; }
    }

    /// <summary>
    /// Identifies a document which is a part of a response. A response consists of multiple documents,
    /// because it may have come from multiple datamarts and each datamart may return multiple documents as well.
    /// </summary>
    [DataContract(Namespace = "http://lincolnpeak.com/schemas/DNS4/API")]
    public class Document
    {
        /// <summary>
        /// Identifier, unique in the context of current communication session
        /// </summary>
        [DataMember]
        public Guid ID { get; set; }

        /// <summary>
        /// Name for this document
        /// </summary>
        [DataMember]
        public string Name { get; set; }


        /// <summary>
        /// Total size of the document, in bytes
        /// </summary>
        [DataMember]
        public long Size { get; set; }

        /// <summary>
        /// MIME type of the document
        /// </summary>
        [DataMember]
        public string MimeType { get; set; }

        /// <summary>
        /// URL that the document may be downloaded from
        /// </summary>
        [DataMember]
        public string LiveUrl { get; set; }
    }

    /// <summary>
    /// This fault means that the requested document was either not found or 
    /// </summary>
    [DataContract(Namespace = "http://lincolnpeak.com/schemas/DNS4/API")]
    public struct DocumentNotFoundFault
    {
        [DataMember]
        public Guid DocumentID { get; set; }
    }
}