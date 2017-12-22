using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;
using System.Web.Mvc;
using System.Diagnostics.Contracts;
using System.Web;

namespace Lpp.Mvc.Controls
{
    public class GridClientSortColumnModel
    {
        public IHtmlString InnerTitle { get; set; }
        public bool AscendingByDefault { get; set; }
    }
}