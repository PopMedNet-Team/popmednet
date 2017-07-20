using System;
using System.Linq;
using System.Web.Mvc;
using Lpp.Dns.Data;
using Lpp.Dns.Portal.Models;
using Lpp.Mvc;
using Lpp.Mvc.Controls;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal.Controllers
{
    interface IOrgChildCrudController
    {
        ComputationResult<Controls.RenderOrgChildrenList> ForSelection();
    }
}
