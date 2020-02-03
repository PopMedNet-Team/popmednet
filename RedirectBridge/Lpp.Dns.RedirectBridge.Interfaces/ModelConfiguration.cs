using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Lpp.Dns
{
    [XmlRoot(Namespace="http://lincolnpeak.com/schemas/DNS4/ModelConfiguration", ElementName="Model")]
    public class ModelConfiguration
    {
        [XmlAttribute] public Guid Id { get; set; }
        [XmlAttribute] public Guid ModelProcessorId { get; set; }
        [XmlAttribute] public string Name { get; set; }
        [XmlAttribute] public string Version { get; set; }
        [XmlElement] public string Description { get; set; }
        [XmlElement(IsNullable=true)] public RSAPublicKey PublicKey { get; set; }
        [XmlElement("RequestType")] public ModelRequestType[] RequestTypes { get; set; }
    }

    public class RSAPublicKey
    {
        [XmlElement] public string ModulusBase64 { get; set; }
        [XmlElement] public string ExponentBase64 { get; set; }
    }

    public class ModelRequestType
    {
        [XmlAttribute] public Guid Id { get; set; }
        [XmlAttribute] public string Name { get; set; }
        [XmlAttribute] public bool IsMetadataRequest { get; set; }
        [XmlElement] public string Description { get; set; }
        [XmlElement] public string CreateRequestUrl { get; set; }
        [XmlElement] public string RetrieveResponseUrl { get; set; }
    }
}