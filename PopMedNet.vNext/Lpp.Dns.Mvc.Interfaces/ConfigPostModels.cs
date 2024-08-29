using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections;

namespace Lpp.Dns
{
    public class ModelConfigurations
    {
        [XmlArrayItem(ElementName = "ConfigModel", Type = typeof(ConfigPostModel))]
        public ArrayList ConfigModels
        {
            get;
            set;
        }
    }

    [XmlRootAttribute("Properties", IsNullable = false)]
    public class ConfigPostModel
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlArrayItem(ElementName = "Property", Type = typeof(ConfigPostProperty))]
        [XmlArray(ElementName = "Properties")]
        public ArrayList Properties
        {
            get;
            set;
        }
    }

    [XmlRootAttribute("Properties", IsNullable = false)]
    public class ConfigPostProperties
    {
        //[XmlArrayItem(ElementName = "Property", Type = typeof(ConfigPostProperty))]
        [XmlElement("Property")]
        public ConfigPostProperty[] Properties
        {
            get;
            set;
        }
    }

    [XmlRootAttribute("Property", Namespace = "", IsNullable = false)]
    public class ConfigPostProperty
    {
        [XmlAttribute]
        public string Name
        {
            get;
            set;
        }

        [XmlAttribute]
        public string Value
        {
            get;
            set;
        }

        public ConfigPostProperty()
        {
        }

        public ConfigPostProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
