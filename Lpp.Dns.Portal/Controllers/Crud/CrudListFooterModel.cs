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
    public class CrudListFooterModel
    {
        public string CreateNewUrl { get; set; }
        public string CreateNewButtonText { get; set; }
    }
}