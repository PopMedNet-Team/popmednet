using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Lpp.Dns.RedirectBridge.Models
{
    public class RedirectPromptModel
    {
        public IDnsRequestContext Context { get; set; }
        public RequestType RequestType { get; set; }
    }
}