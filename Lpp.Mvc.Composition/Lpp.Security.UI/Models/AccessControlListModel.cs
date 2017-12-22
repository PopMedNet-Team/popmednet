using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Composition.Modules;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using Lpp.Mvc;
using System.Web;
using Lpp.Mvc.Application;
using System.Web.Mvc;
using Lpp.Mvc.Controls;

namespace Lpp.Security.UI.Models
{
    public class AccessControlListModel
    {
        public IEnumerable<PrivilegesListModel> InitialEntries { get; set; }
        public IEnumerable<IJsControlledView> PrivilegeEditors { get; set; }
        public AccessControlListParameters Parameters { get; set; }
    }
}