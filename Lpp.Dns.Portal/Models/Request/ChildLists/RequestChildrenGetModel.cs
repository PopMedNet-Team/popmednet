using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Model;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;

namespace Lpp.Dns.Portal.Models
{
    public struct RequestChildrenGetModel : IListGetModel
    {
        public Guid RequestID { get; set; }
        public Guid? ProjectID { get; set; }
        public string Page { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }
        public string PageSize { get; set; }
    }
}