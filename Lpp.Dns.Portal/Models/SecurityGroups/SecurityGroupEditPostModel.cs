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
    public class SecurityGroupEditPostModel<TGroup> : CrudPostModel<TGroup>
    {
        public Guid? ParentID { get; set; }
        public string Name { get; set; }
        public string Parents { get; set; }
    }
}