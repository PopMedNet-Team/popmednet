using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;

namespace Lpp.Dns.Portal.Models
{
    public class SharedRequestViewModel
    {
        public SharedRequestViewModel( Request r, RequestHeader header )
        {
            Request = r;
            Header = header;
        }

        public Request Request { get; private set; }
        public RequestSharedFolder Folder { get; set; }
        public RequestHeader Header { get; set; }
        public IDnsModel Model { get; set; }
        public IDnsRequestType RequestType { get; set; }
        public Func<HtmlHelper,IHtmlString> PluginBody { get; set; }
    }
}