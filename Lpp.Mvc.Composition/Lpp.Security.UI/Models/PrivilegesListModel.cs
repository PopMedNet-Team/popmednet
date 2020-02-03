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
using Lpp.Utilities.Legacy;

namespace Lpp.Security.UI.Models
{
    public class PrivilegesListModel
    {
        public ISecuritySubject Subject { get; set; }
        public IEnumerable<Pair<TargetPrivilegePair, AclEntryKind>> OwnEntries { get; set; }
        public IEnumerable<Pair<TargetPrivilegePair, InheritedPrivilegeModel>> InheritedEntries { get; set; }
    }

    public struct InheritedPrivilegeModel
    {
        public AclEntryKind Kind { get; set; }
        public SecurityTarget InheritedFrom { get; set; }
    }
}