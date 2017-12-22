using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using System.Web.WebPages;
using Lpp.Dns.DTO;

namespace Lpp.Dns.Portal.Models
{
    public class RequestDataMartsListModel
    {
        public ListModel<DataMartListDTO, RequestChildrenGetModel> List { get; set; }
        public IEnumerable<Guid> ProjectIDs { get; set; }
    }
}