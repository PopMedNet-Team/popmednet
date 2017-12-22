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
    public class ResponsesListModel 
    {
        public Request Request { get; set; }
        public ListModel<VirtualResponse, RequestChildrenGetModel> List { get; set; }
        public IEnumerable<IDnsResponseAggregationMode> AggregationModes { get; set; }
        public bool AllowViewResults { get; set; }
        public bool AllowViewIndividualResults { get; set; }
        public bool AllowGroup { get; set; }
        public bool AllowUngroup { get; set; }
        public bool AllowApproval { get; set; }
        public bool AllowResubmit { get; set; }
        public bool ShowHistory { get; set; }
        public bool ShowCheckboxes { get { return AllowViewResults || AllowGroup || AllowUngroup || AllowApproval || AllowResubmit; } }
    }
}