using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using Lpp.Mvc;

namespace Lpp.Dns.Portal.Models
{
    public class GroupListModel : CrudListModel<Group, ListGetModel>
    {

    }
}