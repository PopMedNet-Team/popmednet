using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Model;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using Lpp.Mvc;

namespace Lpp.Dns.Portal.Models
{
    public struct ChildCreateGetModel : ICrudCreateModel
    {
        public Guid ParentID { get; set; }
        public string ReturnTo { get; set; }
    }
}