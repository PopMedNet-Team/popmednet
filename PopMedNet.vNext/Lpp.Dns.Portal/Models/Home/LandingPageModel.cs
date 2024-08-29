using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Audit;
using Lpp.Mvc.Controls;
using Lpp.Audit.UI;
using Lpp.Dns.Model;

namespace Lpp.Dns.Portal.Models
{
    public class LandingPageModel
    {
        public string CurrentVersion { get; set; }
        public IListModel<VisualizedAuditEvent,NotificationsGetModel> Notifications { get; set; }
        public bool AllowTaskList { get; set; }
    }
}