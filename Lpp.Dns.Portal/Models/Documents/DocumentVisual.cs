using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Dns.Portal.Models
{
    public class DocumentVisual
    {
        public Document Document { get; set; }
        public Func<HtmlHelper, IHtmlString> Visual { get; set; }
    }
}