using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using Lpp.Mvc;
using System.Web.Mvc;
using System.Diagnostics.Contracts;
using System.Web;

namespace Lpp.Mvc.Controls
{
    public class GridCheckboxColumnModel
    {
        public string HiddenFieldName { get; set; }
        public string JsFnGetAllPossibleIdsCommaSeparated { get; set; }
    }
}