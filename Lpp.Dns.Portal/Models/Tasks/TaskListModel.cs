using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Lpp.Dns.Model;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using System.ComponentModel;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal.Models
{
    public class TaskListModel
    {
        internal ListModel<TaskListRowModel, TaskListGetModel> List { get; set; }
        internal TaskStatusFilter StatusFilter { get { return TaskStatusFilter.All; } }
        public string ReturnTo { get; set; }
    }

    public struct TaskListGetModel : IListGetModel
    {
        public string Page { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }
        public string PageSize { get; set; }
        public TaskStatusFilter? StatusFilter { get; set; }
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }
    }

    public enum TaskStatusFilter
    {
        [Description("All tasks")]
        All,

        [Description("Awaiting approval")]
        AwaitingApproval,

        [Description("Approved")]
        Approved,

        [Description("Rejected")]
        Rejected,

        //[Description("Drafts only")]
        //DraftsOnly,

        //[Description( "Submitted only" )]
        //SubmittedOnly,

        //[Description( "Partially complete" )]
        //PartiallyComplete,

        //[Description( "Fully complete" )]
        //Complete,

        //[Description( "Awaiting Approval" )]
        //Approval,

        //[Description( "Scheduled" )]
        //Scheduled
    }

    public class TaskListRowModel
    {
        internal User User { get; set; }
        internal Request Request { get; set; }
        internal int CompletedDataMarts { get; set; }
        internal long TotalDataMarts { get; set; }
        internal bool UnapprovedRoutings { get; set; }
        internal bool UnapprovedResults { get; set; }
        internal bool RejectedRoutings { get; set; }
        internal bool RejectedResults { get; set; }
    }

}