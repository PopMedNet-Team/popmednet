using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using Lpp.Composition;
using Lpp.Data;
using Lpp.Dns.DocumentVisualizers.Views;
using Lpp.Mvc;
using Lpp.Dns.Portal;
using Lpp.Dns.Model;

namespace Lpp.Dns.PubHealth.Models
{
    public class PubHealthModel
    {
        public PubHealthModel()
        {
        }
        public TestRequestType RequestType { get; set; }
        public int RequestId { get; set; }
        public string SqlQuery { get; set; }

    }
}