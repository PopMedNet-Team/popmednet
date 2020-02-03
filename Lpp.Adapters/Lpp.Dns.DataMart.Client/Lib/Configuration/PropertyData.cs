using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Lpp.Dns.DataMart.Lib
{
    [XmlRootAttribute("Property", Namespace = "", IsNullable = false)]
    [Serializable()]
    public class PropertyData
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

        public PropertyData()
        {
        }

        public PropertyData(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    [XmlRootAttribute("Properties", IsNullable = false)]
    public class ModelProperties
    {
        [XmlElement("Property")]
        public PropertyData[] Properties
        {
            get;
            set;
        }
    }
}
