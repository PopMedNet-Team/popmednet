using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Lpp.Dns.DataMart.Model
{
    [XmlRootAttribute("ViewableDocumentStyle", Namespace = "", IsNullable = false)]
    public class ViewableDocumentStyle
    {
        public string Css { get; set; }
        public int[] StyledRows { get; set; }
    }
}
