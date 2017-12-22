using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml.Serialization;

namespace Lpp.Dns.DataMart.Lib
{
    /// <summary>
    /// Class that holds datamart specific settings
    /// </summary>
    [XmlRootAttribute("ModelDescription", Namespace = "", IsNullable = false)]
    [Serializable()]
    public class ModelDescription
    {

        #region Constructor
        public ModelDescription()
        {
        }
        #endregion

        #region Properties

        [XmlAttribute]
        public Guid ModelId
        {
            get;
            set;
        }

        [XmlAttribute]
        public string ModelName
        {
            get;
            set;
        }

        [XmlAttribute]
        public Guid ProcessorId
        {
            get;
            set;
        }

        [XmlAttribute]
        public string ProcessorPath
        {
            get;
            set;
        }

        [XmlAttribute]
        public string ClassName
        {
            get;
            set;
        }

        [XmlIgnore]
        public string ModelDisplayName
        {
            get;
            set;
        }

        [XmlArrayItem(ElementName = "Property", Type = typeof(PropertyData))]
        [XmlArray(ElementName = "Properties")]
        public List<PropertyData> Properties
        {
            get;
            set;
        }

        #endregion
    }
}
