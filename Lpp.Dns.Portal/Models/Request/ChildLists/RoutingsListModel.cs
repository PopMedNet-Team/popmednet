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
    public class RoutingsListModel 
    {
        public Request Request { get; set; }
        public ListModel<Response, RequestChildrenGetModel> List { get; set; }
        public bool AllowChangeRoutings { get; set; }
        public bool ShowHistory { get; set; }
    }
}