using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Lpp.Dns.RedirectBridge.Models
{
    public class UploadConfirmModel
    {
        public ModelConfiguration Configuration { get; set; }
        public string Serialized { get; set; }
    }
}