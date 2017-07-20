using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal.Models
{
    public class ResponseViewModel
    {
        public Request Request { get; set; }
        public IDnsRequestType RequestType { get; set; }
        public IEnumerable<IDnsDataMartResponse> Responses { get; set; }
        public string ResponseToken { get; set; }
        public string AggregationMode { get; set; }
        public bool AllowGroup { get; set; }
        public bool AllowApprove { get; set; }
        public bool AllowUngroup { get; set; }
        public IEnumerable<IDnsResponseExportFormat> ExportFormats { get; set; }
        public Func<HtmlHelper, IHtmlString> BodyView { get; set; }
        public Func<HtmlHelper, IHtmlString> RequestBodyView { get; set; }
        public string RequesterCenterName { get; set; }
        public string WorkplanTypeName { get; set; }
        public string ReportAggregationLevelName { get; set; }
    }
}