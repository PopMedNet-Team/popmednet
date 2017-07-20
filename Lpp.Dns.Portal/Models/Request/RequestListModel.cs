using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;
using Lpp.Mvc.Controls;
using System.ComponentModel;
using System.Linq.Expressions;
using Lpp.Dns.Portal.Controllers;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal.Models
{
    public class RequestListModel
    {
        public Project Project { get; set; }
        public Guid? ProjectID { get { return Project == null ? null : (Guid?) Project.ID; } }
        public string Folder { get; set; }
        public ListModel<RequestListRowModel, RequestListGetModel> List { get; set; }
        public IDictionary<Guid, PluginRequestType> GrantedRequestTypes { get; set; }
        public IDictionary<Guid, PluginRequestType> AllRequestTypes { get; set; }
        public IEnumerable<PluginRequestType> UsedRequestTypes { get; set; }
        public IEnumerable<Project> AllProjects { get; set; }
        public RequestStatusFilter StatusFilter { get { return List.OriginalRequest.StatusFilter ?? RequestStatusFilter.All; } }
        public PluginRequestType RequestTypeFilter { get { return List.OriginalRequest.RequestTypeFilter == null ? null : GrantedRequestTypes.ValueOrDefault( List.OriginalRequest.RequestTypeFilter.Value ); } }
    }

    public struct RequestListGetModel : IListGetModel
    {
        public string Page { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }
        public string PageSize { get; set; }

        public Guid? ProjectID { get; set; }
        public string Folder { get; set; }
        public RequestSearchFolder? SearchFolder { get { return Maybe.ParseEnum<RequestSearchFolder>( Folder ).AsNullable(); } }

        public RequestStatusFilter? StatusFilter { get; set; }
        public Guid? RequestTypeFilter { get; set; }
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }
        public string SettingsContext { get; set; }
        public bool? HideStatusFilter { get; set; }
        public bool? HideDateFilter { get; set; }
    }

    public enum RequestStatusFilter
    {
        [Description("All requests")]
        All,

        [Description("Drafts only")]
        DraftsOnly,

        [Description( "Submitted only" )]
        SubmittedOnly,

        [Description( "Partially complete" )]
        PartiallyComplete,

        [Description( "Fully complete" )]
        Complete,

        [Description( "Awaiting Approval" )]
        Approval,

        [Description( "Scheduled" )]
        Scheduled,

        // The following filters don't have a [Description] attribute,
        // which will prevent them from showing up in the drop-down list
        // when choosing filter on the request grid. They are, however,
        // used in the Network Browser tree for search folders
        PartiallyOrFullyComplete
    }

    public class RequestListRowModel
    {
        public Request Request { get; set; }
        public int CompletedDataMarts { get; set; }
        public long TotalDataMarts { get; set; }
        public bool UnapprovedRoutings { get; set; }
        public bool UnapprovedResults { get; set; }
        public bool RejectedRoutings { get; set; }
        public bool RejectedResults { get; set; }

        public static Expression<Func<Request, RequestListRowModel>> FromRequest 
        { 
            get
            {
                return _fromRequest; 
            }
        }

        readonly static Expression<Func<Request, RequestListRowModel>> _fromRequest = r => new RequestListRowModel
        {
            Request = r,
            TotalDataMarts = r.Statistics.Total,
            CompletedDataMarts = r.Statistics.Completed,
            UnapprovedRoutings = r.DataMarts.Any(dm => dm.Status == DTO.Enums.RoutingStatus.AwaitingRequestApproval),
            UnapprovedResults = r.DataMarts.Any(dm => dm.Status == DTO.Enums.RoutingStatus.AwaitingResponseApproval || dm.Status == DTO.Enums.RoutingStatus.Hold),
            RejectedRoutings = (r.Statistics.RejectedRequest) > 0,
            RejectedResults = r.Statistics.RejectedBeforeUploadResults > 0 || r.Statistics.RejectedAfterUploadResults > 0,
        };
        
    }
}