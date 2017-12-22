using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics.Contracts;

namespace Lpp.Mvc.Controls
{
    public class MenuItemModel
    {
        public string Title { get; set; }
        public string Target { get; set; }
        public IEnumerable<MenuItemModel> Children { get; set; }
    }
}