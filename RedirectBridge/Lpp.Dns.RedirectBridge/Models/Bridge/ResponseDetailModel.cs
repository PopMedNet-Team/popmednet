using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Lpp.Dns.RedirectBridge.Models
{
    public class ResponseDetailModel
    {
        public IDnsResponseContext Context { get; set; }
        public Model Model { get; set; }
        public RequestType RequestType { get; set; }
    }
}