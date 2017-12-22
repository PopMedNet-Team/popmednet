using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using Lpp.Dns.Model;
using Lpp.Dns.Portal.Models;
using Lpp.Mvc.Controls;
using Lpp.Security;

namespace Lpp.Dns.Portal
{
    public interface ICrudSecObjectEditModel
    {
        bool AllowSave { get; set; }
        bool AllowDelete { get; set; }
        bool ShowAcl { get; set; }
    }
}