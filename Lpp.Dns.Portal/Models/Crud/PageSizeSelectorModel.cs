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
    public class PageSizeSelectorModel
    {
        public int CurrentSize { get; set; }
        public string CurrentText { get; set; }

        public IEnumerable<int> Options { get; set; }
        public Func<int, string> Text { get; set; }
        public Func<int, string> ReloadUrl { get; set; }
    }
}