using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Mvc.Controls;

namespace Lpp.Dns.Portal.Models
{
    public class RequestViewModel
    {
        public Guid RequestID { get { return Request.ID; } }
        public Request Request { get; set; }
        //public INetworkTreeNode TreeNode { get; set; }
        public string OriginalFolder { get; set; }
        public IDnsRequestType RequestType { get; set; }
        public RoutingsListModel Routings { get; set; }
        public ResponsesListModel Responses { get; set; }
        public ListModel<DataMartListDTO, RequestChildrenGetModel> UnassignedDataMarts { get; set; }
        public Func<HtmlHelper, IHtmlString> BodyView { get; set; }

        public bool IsScheduled { get; set; }
        public bool AllowApprove { get; set; }
        public bool AllowCopy { get; set; }
        public bool AllowMetadataEdit { get; set; }
        public bool AllowEditRoutingStatus { get; set; }
        public bool AllowEditRequestID { get; set; }
        public Activity ParentActivity { get; set; }

        public string RequesterCenterName { get; set; }
        public string WorkplanTypeName { get; set; }
        public string ReportAggregationLevelName { get; set; }

    }

    public class RequestIFrameViewModel : RequestViewModel
    {
        public string IFrameUrl { get; set; }
    }
}