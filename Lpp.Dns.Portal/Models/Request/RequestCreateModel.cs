using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Model;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using Lpp.Dns.Portal.Controllers;

namespace Lpp.Dns.Portal.Models
{
    public class RequestCreateModel
    {
        internal IEnumerable<PluginRequestType> RequestTypes { get; set; }
    }
}