using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;
using Lpp.Mvc.Controls;
using Lpp.Dns.DTO;

namespace Lpp.Dns.Portal.Models
{
    public class RequestEditModel
    {
        public RequestEditModel( Request request, RequestHeader header )
        {
            Request = new RequestDetailForEditModel { 
                ID= request.ID,
                Name = request.Name,
                Priority = request.Priority,
                DueDate = request.DueDate,
                ProjectID = request.ProjectID,
                ProjectName = request.Project.Name,
                ActivityID = request.ActivityID,
                SourceActivityID = request.SourceActivityID,
                SourceActivityProjectID = request.SourceActivityProjectID,
                SourceTaskOrderID = request.SourceTaskOrderID,
                Scheduled = request.Scheduled,
                MirrorBudgetFields = request.MirrorBudgetFields,
                MSRequestID = request.MSRequestID,
                ReportAggregationLevelID = request.ReportAggregationLevelID
            };            
            if (request.Activity != null)
            {
                Request.Activity = new RequestDetailActivityViewModel(request.Activity);
            }
            Header = header;
        }

        public RequestDetailForEditModel Request { get; private set; }
        public string OriginalFolder { get; set; }
        public RequestHeader Header { get; set; }
        public IDnsModel Model { get; set; }
        public IEnumerable<DataMartListDTO> DataMarts { get; set; }
        public string SelectedDataMarts { get; set; }
        public IEnumerable<RequestDataMartDTO> SelectedRequestDataMarts { get; set; }
        public IDnsRequestType RequestType { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public Func<HtmlHelper,IHtmlString> PluginBody { get; set; }
        public IEnumerable<ActivityDTO> Activities { get; set; }
        public bool AllowSubmit { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowEditRequestID { get; set; }
        public RequestScheduleModel Schedule { get; set; }
        public IEnumerable<ProjectDTO> Projects { get; set; }
        public IEnumerable<RequesterCenter> RequesterCenters { get; set; }
        public IEnumerable<WorkplanType> WorkplanTypes { get; set; }
        public IEnumerable<ReportAggregationLevel> ReportAggregationLevels { get; set; }
    }

    //A flattened class with only the information required for the viewing the edit page.
    public class RequestDetailForEditModel
    {
        public Guid ID { get; set; }
        
        public string Name { get; set; }

        public DTO.Enums.Priorities Priority { get; set; }

        public DateTime? DueDate { get; set; }

        public Guid ProjectID { get; set; }

        public string ProjectName { get; set; }

        public Guid? ActivityID { get; set; }

        public Guid? SourceActivityID { get; set; }

        public Guid? SourceActivityProjectID { get; set; }

        public Guid? SourceTaskOrderID { get; set; }

        public RequestDetailActivityViewModel Activity { get; set; }

        public bool Scheduled { get; set; }

        public bool MirrorBudgetFields { get; set; }

        public string MSRequestID { get; set; }

        public Guid? ReportAggregationLevelID { get; set; }
    }

    public class RequestDetailActivityViewModel
    {
        public RequestDetailActivityViewModel(Activity activity)
        {
            ID = activity.ID;
            Name = activity.Name;
            Description = activity.Description;
            DisplayOrder = activity.DisplayOrder;
            TaskLevel = activity.TaskLevel;
            ParentActivityID = activity.ParentActivityID;
            if (activity.ParentActivity != null)
            {
                ParentActivity = new RequestDetailActivityViewModel(activity.ParentActivity);
            }
        }
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public int TaskLevel { get; set; }

        public Guid? ParentActivityID { get; set; }

        public RequestDetailActivityViewModel ParentActivity { get; set; }
    }
}