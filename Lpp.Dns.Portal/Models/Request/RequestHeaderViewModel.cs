using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal.Models
{
    public class RequestHeaderViewModel
    {
        public Request Request { get; set; }
        public IDnsRequestType Type { get; set; }
        public IEnumerable<RequesterCenter> RequesterCenters { get; set; }
        public IEnumerable<WorkplanType> WorkplanTypes { get; set; }
        public IEnumerable<ReportAggregationLevel> ReportAggregationLevels { get; set; }

        public string RequesterCenterName { get; set; }
        public string WorkplanTypeName { get; set; }
        public string ReportAggregationLevelName { get; set; }
    }
}