using System.ComponentModel.Composition;
using System.Linq;
using Lpp.Dns.Model;
using Lpp.Dns.Data;
using Lpp.Dns.Portal.Models;
using Lpp.Mvc;
using Lpp.Mvc.Controls;
using Lpp.Security;
using Lpp.Security.UI;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal.Controllers
{
    interface ISecurityGroupAuthorityController<TParent>
    {
        SortHelper<TParent> Sort { get; }
        ActionResult Edit(Guid id, string returnTo);
    }
}
