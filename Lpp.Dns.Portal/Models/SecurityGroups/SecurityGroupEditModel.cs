using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using Lpp.Security.UI;
using Lpp.Mvc;

namespace Lpp.Dns.Portal.Models
{
    public class SecurityGroupEditModel : ICrudSecObjectEditModel
    {
        public SecurityGroup SecurityGroup { get; set; }
        public bool AllowSave { get; set; }
        public bool AllowDelete { get; set; }
        public bool ShowAcl { get; set; }
        public Func<HtmlHelper, IJsControlledView> SecGroupSelector { get; set; }

        public string ReturnTo { get; set; }
    }
}