using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;

namespace Lpp.Dns.Portal.Models
{
    public class TemplateViewModel
    {
        public Request Request { get; set; }
        public Func<HtmlHelper, IHtmlString> BodyView { get; set; }
        public RequestDataMartsListModel DataMarts { get; set; }
        public IDnsRequestType RequestType { get; set; }
        public RequestScheduleModel SchedulerModel { get; set; }

        public bool AllowCopy { get; set; }
    }
}