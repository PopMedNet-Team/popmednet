﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lpp.Dns.General.Metadata.Views
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Lpp;
    using Lpp.Dns.General.CriteriaGroup;
    using Lpp.Dns.General.CriteriaGroup.Models;
    
    #line 2 "..\..\Views\DisplayRequest.cshtml"
    using Lpp.Dns.Portal;
    
    #line default
    #line hidden
    
    #line 1 "..\..\Views\DisplayRequest.cshtml"
    using Lpp.Mvc;
    
    #line default
    #line hidden
    using Lpp.Mvc.Application;
    using Lpp.Mvc.Controls;
    using Lpp.Utilities.Legacy;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/DisplayRequest.cshtml")]
    public partial class DisplayRequest : System.Web.Mvc.WebViewPage<Lpp.Dns.General.Metadata.Models.MetadataSearchModel>
    {
        public DisplayRequest()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Views\DisplayRequest.cshtml"
  
    this.Stylesheet("MetadataSearch.css");
    var id = Html.UniqueId();

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n    var KOViewModel = true;     // indicates this page is built with KO templa" +
"tes\r\n</script>\r\n\r\n<div");

WriteLiteral(" id=\"AgeRangeTerm\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral(">\r\n    <span>Age Range</span>\r\n    <div");

WriteLiteral(" class=\"ui-form\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"Field\"");

WriteLiteral(">\r\n            <label>Minimum Age</label>\r\n            <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"value: MinAge\"");

WriteLiteral(" style=\"width: 25px; margin-right: 10px;\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"Field\"");

WriteLiteral(">\r\n            <label>Maximum Age</label>\r\n            <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"value: MaxAge\"");

WriteLiteral(" style=\"width: 25px;\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"AgeStratifierTerm\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral(">\r\n    <label>Age Stratifier</label>\r\n    <select");

WriteLiteral(" data-bind=\"options: RequestCriteriaViewModels.AgeStratifierTerm.AgeStratifiersLi" +
"st,\r\n        optionsText: \'Key\',\r\n        optionsValue: \'Value\',\r\n        value:" +
" AgeStratifier\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(">\r\n    </select>\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"ClinicalSettingTerm\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral(">\r\n    <label>Clinical Setting</label>\r\n    <select");

WriteLiteral(" data-bind=\"options: RequestCriteriaViewModels.ClinicalSettingTerm.ClinicalSettin" +
"gsList,\r\n        optionsText: \'Key\',\r\n        optionsValue: \'Value\',\r\n        va" +
"lue: ClinicalSetting\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(">\r\n    </select>\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"CodesTerm\"");

WriteLiteral(" style=\"display: none;\"");

WriteLiteral(">\r\n    <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" id=\"CodesTerm_Codes\"");

WriteLiteral(" name=\"CodesTerm_Codes\"");

WriteLiteral(" data-bind=\"value: Codes\"");

WriteLiteral(" />\r\n    <label>Code Set</label>\r\n\r\n    <select");

WriteLiteral(" data-bind=\"options: MetadataQuery.Create.ViewModel.MDQCodeSetList,\r\n        opti" +
"onsText: \'Key\',\r\n        optionsValue: \'Value\',\r\n        value: CodesTermType\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(">\r\n    </select>\r\n\r\n    <label>Codes</label> <span");

WriteLiteral(" data-bind=\"text: Codes\"");

WriteLiteral("> </span>\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"DateRangeTerm\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" style=\"width: 240px;\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" data-bind=\"text: Title\"");

WriteLiteral("></label>\r\n        <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"value: StartDate() != null ? moment.utc(StartDate()).local().format(\'" +
"MM/DD/YYYY\') : \'\'\"");

WriteLiteral(" style=\"width: 100px;\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />&nbsp;-&nbsp;\r\n        <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"value: EndDate() != null ? moment.utc(EndDate()).local().format(\'MM/D" +
"D/YYYY\') : \'\'\"");

WriteLiteral(" style=\"width: 100px;\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n    </div>\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"ProjectTerm\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral(">\r\n    <label>Project</label>\r\n    <select");

WriteLiteral(" data-bind=\"options: $.parseJSON(\'");

            
            #line 68 "..\..\Views\DisplayRequest.cshtml"
                                         Write(Json.Encode(Model.Projects));

            
            #line default
            #line hidden
WriteLiteral("\'),\r\n        optionsText: \'Key\',\r\n        optionsValue: \'Value\',\r\n        value: " +
"Project\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(">\r\n    </select>\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"RequestStatusTerm\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral(">\r\n    <label>Request Status</label>\r\n    <select");

WriteLiteral(" style=\"width:270px; \"");

WriteLiteral(" data-bind=\"options: MetadataQuery.Create.ViewModel.RequestStatusList,\r\n        o" +
"ptionsText: \'text\',\r\n        optionsValue: \'value\',\r\n        value: RequestStatu" +
"s\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(">\r\n    </select>\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"SexTerm\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral(">\r\n    <label>Sex</label>\r\n    <select");

WriteLiteral(" data-bind=\"options: RequestCriteriaViewModels.SexTerm.SexesList,\r\n        option" +
"sText: \'Key\',\r\n        optionsValue: \'Value\',\r\n        value: Sex\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(">\r\n    </select>\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"RequesterCenterTerm\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral(">\r\n    <label>Requester Center</label>\r\n    <select");

WriteLiteral(" data-bind=\"options: RequestCriteriaViewModels.RequestCriteria.RequesterCenterLis" +
"t,\r\n        optionsText: \'Value\',\r\n        optionsValue: \'Key\',\r\n        value: " +
"RequesterCenter\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral("></select>\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"WorkplanTypeTerm\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral(">\r\n    <label>Workplan Type</label>\r\n    <select");

WriteLiteral(" style=\"width:270px; \"");

WriteLiteral(" data-bind=\"options: RequestCriteriaViewModels.RequestCriteria.WorkplanTypeList,\r" +
"\n        optionsText: \'Value\',\r\n        optionsValue: \'Key\',\r\n        value: Wor" +
"kplanType\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral("></select>\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"ReportAggregationLevelTerm\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral(">\r\n    <label>Level of Report Aggregation</label>\r\n    <select");

WriteLiteral(" data-bind=\"options: RequestCriteriaViewModels.RequestCriteria.ReportAggregationL" +
"evelList,\r\n        optionsText: \'Value\',\r\n        optionsValue: \'Key\',\r\n        " +
"value: ReportAggregationLevel\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral("></select>\r\n</div>\r\n\r\n<div");

WriteLiteral(" class=\"MetadataSearch ui-form\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"ui-form\"");

WriteLiteral(">\r\n        <fieldset");

WriteLiteral(" id=\"fsCriteria\"");

WriteLiteral(" style=\"padding: 0px; margin: 0px;\"");

WriteLiteral(">\r\n            <legend");

WriteLiteral(" style=\"display: none;\"");

WriteLiteral("></legend>\r\n\r\n            ");

WriteLiteral("\r\n            <div");

WriteLiteral(" id=\'errorLocation\'");

WriteLiteral(" style=\"font-size: small; color: Gray;\"");

WriteLiteral("></div>\r\n            <div");

WriteLiteral(" class=\"ui-groupbox\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"ui-groupbox-header\"");

WriteLiteral(">\r\n                    <span>Search Criteria</span>\r\n                </div>\r\n    " +
"            <ol");

WriteLiteral(" data-bind=\"foreach: RequestCriteria.Criterias\"");

WriteLiteral(">\r\n                    <li>\r\n                        <ul");

WriteLiteral(" data-bind=\"foreach: HeaderTerms\"");

WriteLiteral(">\r\n                            <li");

WriteLiteral(" class=\"col-xs-3\"");

WriteLiteral(">\r\n                                <div");

WriteLiteral(" data-bind=\"template: { name: TemplateName }\"");

WriteLiteral("></div>\r\n                            </li>\r\n                        </ul>\r\n      " +
"              </li>\r\n                </ol>\r\n                <br");

WriteLiteral(" style=\"clear: both;\"");

WriteLiteral(" />\r\n                <ol>\r\n                    <li>\r\n                        <br");

WriteLiteral(" style=\"clear: both;\"");

WriteLiteral(" />\r\n                        <ul>\r\n                            <li");

WriteLiteral(" class=\"col-xs-3\"");

WriteLiteral(" id=\"SourceTaskOrderSearchTerm\"");

WriteLiteral(">\r\n                                <label>Source Task Order</label>\r\n            " +
"                    <input");

WriteLiteral(" id=\"cboSearchSourceTask\"");

WriteLiteral(" data-bind=\"kendoDropDownList: { value:SearchSourceTaskOrderID, data:SourceTaskAc" +
"tivities.dsTaskOrders, dataTextField:\'ActivityName\', dataValueField:\'ActivityID\'" +
", optionLabel:{ ActivityName:\'Not Selected\', ActivityID:\'\' }, autoBind:true}\"");

WriteLiteral(" style=\"width:100%;\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                            </li>\r\n                            <li");

WriteLiteral(" class=\"col-xs-3\"");

WriteLiteral(" id=\"SourceActivitySearchTerm\"");

WriteLiteral(">\r\n                                <label>Source Activity</label>\r\n              " +
"                  <input");

WriteLiteral(" id=\"cboSearchSourceActivity\"");

WriteLiteral(@" data-bind=""enable: false, kendoDropDownList: { enable: false, data:SourceTaskActivities.dsActivities, dataTextField:'ActivityName', dataValueField:'ActivityID', cascadeFrom: 'cboSearchSourceTask', cascadeFromField: 'ParentID', optionLabel: { ActivityName: 'Not Selected', ActivityID: ''}, autoBind: false}, value: SearchSourceActivityID""");

WriteLiteral(" style=\"width:100%;\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                            </li>\r\n                            <li");

WriteLiteral(" class=\"col-xs-3\"");

WriteLiteral(" id=\"SourceActivityProjectSearchTerm\"");

WriteLiteral(">\r\n                                <label>Source Activity Project</label>\r\n      " +
"                          <input");

WriteLiteral(" id=\"cboSearchSourceActivityProject\"");

WriteLiteral(@" data-bind=""enable: false, kendoDropDownList: {enable: false, data:SourceTaskActivities.dsActivityProjects, dataTextField:'ActivityName', dataValueField:'ActivityID', cascadeFrom: 'cboSearchSourceActivity', cascadeFromField: 'ParentID', optionLabel: { ActivityName: 'Not Selected', ActivityID: ''}, autoBind: false}, value: SearchSourceActivityProjectID""");

WriteLiteral(" style=\"width:100%;\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                            </li>\r\n                        </ul>\r\n          " +
"          </li>\r\n                </ol>\r\n                <br");

WriteLiteral(" style=\"clear: both;\"");

WriteLiteral(" />\r\n                <ol>\r\n                    <li>\r\n                        <ul>" +
"\r\n                            <li");

WriteLiteral(" class=\"col-xs-3\"");

WriteLiteral(" id=\"TaskOrderSearchTerm\"");

WriteLiteral(">\r\n                                <label>Budget Task Order</label>\r\n            " +
"                    <input");

WriteLiteral(" id=\"cboSearchTask\"");

WriteLiteral(" data-bind=\"kendoDropDownList: { value:SearchTaskOrderID, data:TaskActivities.dsT" +
"askOrders, dataTextField:\'ActivityName\', dataValueField:\'ActivityID\', optionLabe" +
"l:{ ActivityName:\'Not Selected\', ActivityID:\'\' }, autoBind:true}\"");

WriteLiteral(" style=\"width:100%;\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                            </li>\r\n                            <li");

WriteLiteral(" class=\"col-xs-3\"");

WriteLiteral(" id=\"ActivitySearchTerm\"");

WriteLiteral(">\r\n                                <label>Budget Activity</label>\r\n              " +
"                  <input");

WriteLiteral(" id=\"cboSearchActivity\"");

WriteLiteral(@" data-bind="" enable: false, kendoDropDownList: { enable: false, data:TaskActivities.dsActivities, dataTextField:'ActivityName', dataValueField:'ActivityID', cascadeFrom: 'cboSearchTask', cascadeFromField: 'ParentID', optionLabel: { ActivityName: 'Not Selected', ActivityID: ''}, autoBind: false}, value: SearchActivityID""");

WriteLiteral(" style=\"width:100%;\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                            </li>\r\n                            <li");

WriteLiteral(" class=\"col-xs-3\"");

WriteLiteral(" id=\"ActivityProjectSearchTerm\"");

WriteLiteral(">\r\n                                <label>Budget Activity Project</label>\r\n      " +
"                          <input");

WriteLiteral(" id=\"cboSearchActivityProject\"");

WriteLiteral(@" data-bind=""enable: false, kendoDropDownList: { enable: false, data:TaskActivities.dsActivityProjects, dataTextField:'ActivityName', dataValueField:'ActivityID', cascadeFrom: 'cboSearchActivity', cascadeFromField: 'ParentID', optionLabel: { ActivityName: 'Not Selected', ActivityID: ''}, autoBind: false}, value: SearchActivityProjectID""");

WriteLiteral(" style=\"width:100%;\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                            </li>\r\n                        </ul>\r\n          " +
"          </li>\r\n                </ol>\r\n                <br");

WriteLiteral(" style=\"clear: both;\"");

WriteLiteral(" />\r\n                <hr");

WriteLiteral(" style=\"color: #FFFFFF; background-color: #FFFFFF;\"");

WriteLiteral(">\r\n                <ol");

WriteLiteral(" data-bind=\"foreach: RequestCriteria.Criterias\"");

WriteLiteral(">\r\n                    <li>\r\n                        <ul");

WriteLiteral(" data-bind=\"foreach: RequestTerms\"");

WriteLiteral(">\r\n                            <li");

WriteLiteral(" class=\"col3\"");

WriteLiteral(">\r\n                                <div");

WriteLiteral(" data-bind=\"template: { name: TemplateName }\"");

WriteLiteral("></div>\r\n                            </li>\r\n                        </ul>\r\n      " +
"              </li>\r\n                </ol>\r\n                <br");

WriteLiteral(" style=\"clear: both;\"");

WriteLiteral(" />\r\n            </div>\r\n        </fieldset>\r\n    </div>\r\n</div>\r\n\r\n<script");

WriteAttribute("src", Tuple.Create(" src=\"", 9933), Tuple.Create("\"", 9984)
            
            #line 193 "..\..\Views\DisplayRequest.cshtml"
, Tuple.Create(Tuple.Create("", 9939), Tuple.Create<System.Object, System.Int32>(this.Resource("Models/DataCheckerModels.js")
            
            #line default
            #line hidden
, 9939), false)
);

WriteLiteral("></script>\r\n<script");

WriteAttribute("src", Tuple.Create(" src=\"", 10004), Tuple.Create("\"", 10063)
            
            #line 194 "..\..\Views\DisplayRequest.cshtml"
, Tuple.Create(Tuple.Create("", 10010), Tuple.Create<System.Object, System.Int32>(this.Resource("ViewModels/DataCheckerViewModels.js")
            
            #line default
            #line hidden
, 10010), false)
);

WriteLiteral("></script>\r\n<script");

WriteAttribute("src", Tuple.Create(" src=\"", 10083), Tuple.Create("\"", 10138)
            
            #line 195 "..\..\Views\DisplayRequest.cshtml"
, Tuple.Create(Tuple.Create("", 10089), Tuple.Create<System.Object, System.Int32>(this.Resource("Models/RequestCriteriaModels.js")
            
            #line default
            #line hidden
, 10089), false)
);

WriteLiteral("></script>\r\n<script");

WriteAttribute("src", Tuple.Create(" src=\"", 10158), Tuple.Create("\"", 10221)
            
            #line 196 "..\..\Views\DisplayRequest.cshtml"
, Tuple.Create(Tuple.Create("", 10164), Tuple.Create<System.Object, System.Int32>(this.Resource("ViewModels/RequestCriteriaViewModels.js")
            
            #line default
            #line hidden
, 10164), false)
);

WriteLiteral("></script>\r\n\r\n");

WriteLiteral("\r\n<script");

WriteAttribute("src", Tuple.Create(" src=\"", 10267), Tuple.Create("\"", 10300)
            
            #line 199 "..\..\Views\DisplayRequest.cshtml"
, Tuple.Create(Tuple.Create("", 10273), Tuple.Create<System.Object, System.Int32>(this.Resource("Create.js")
            
            #line default
            #line hidden
, 10273), false)
);

WriteLiteral("></script>\r\n\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n    jQuery(document).ready(function () {\r\n        // initialize the viewmodel\r" +
"\n        var json = \'");

            
            #line 204 "..\..\Views\DisplayRequest.cshtml"
                Write(Html.Raw(Model.CriteriaGroupsJSON));

            
            #line default
            #line hidden
WriteLiteral("\' || \'{}\';\r\n\r\n        var activityjson = \'");

            
            #line 206 "..\..\Views\DisplayRequest.cshtml"
                       Write(Html.Raw(Json.Encode(Model.AllActivities)));

            
            #line default
            #line hidden
WriteLiteral("\' || \'{}\';\r\n        var workplanTypeJson = \'");

            
            #line 207 "..\..\Views\DisplayRequest.cshtml"
                           Write(Html.Raw(Json.Encode(Model.WorkplanTypeList)));

            
            #line default
            #line hidden
WriteLiteral("\' || \'{}\';\r\n        var requesterCenterJson = \'");

            
            #line 208 "..\..\Views\DisplayRequest.cshtml"
                              Write(Html.Raw(Json.Encode(Model.RequesterCenterList)));

            
            #line default
            #line hidden
WriteLiteral("\' || \'{}\';\r\n        var reportAggregationLevelJson = \'");

            
            #line 209 "..\..\Views\DisplayRequest.cshtml"
                                     Write(Html.Raw(Json.Encode(Model.ReportAggregationLevelList)));

            
            #line default
            #line hidden
WriteLiteral("\' || \'{}\';\r\n        var taskOrder = \'");

            
            #line 210 "..\..\Views\DisplayRequest.cshtml"
                    Write(Model.TaskOrder);

            
            #line default
            #line hidden
WriteLiteral("\';\r\n        var activity = \'");

            
            #line 211 "..\..\Views\DisplayRequest.cshtml"
                   Write(Model.Activity);

            
            #line default
            #line hidden
WriteLiteral("\';\r\n        var activityProject = \'");

            
            #line 212 "..\..\Views\DisplayRequest.cshtml"
                          Write(Model.ActivityProject);

            
            #line default
            #line hidden
WriteLiteral("\';\r\n        var sourceTaskOrder = \'");

            
            #line 213 "..\..\Views\DisplayRequest.cshtml"
                          Write(Model.SourceTaskOrder);

            
            #line default
            #line hidden
WriteLiteral("\';\r\n        var sourceActivity = \'");

            
            #line 214 "..\..\Views\DisplayRequest.cshtml"
                         Write(Model.SourceActivity);

            
            #line default
            #line hidden
WriteLiteral("\';\r\n        var sourceActivityProject = \'");

            
            #line 215 "..\..\Views\DisplayRequest.cshtml"
                                Write(Model.SourceActivityProject);

            
            #line default
            #line hidden
WriteLiteral(@"';

        MetadataQuery.Create.init($.parseJSON(json), $(""#fsCriteria""), $(""#CriteriaGroupsJSON""), $.parseJSON(activityjson), $.parseJSON(workplanTypeJson), $.parseJSON(requesterCenterJson), $.parseJSON(reportAggregationLevelJson),
            taskOrder, activity, activityProject, sourceTaskOrder, sourceActivity, sourceActivityProject);

        var dropdownlist = $(""#cboSearchActivity"").data(""kendoDropDownList"");
        dropdownlist.open();
        dropdownlist.close();
        var sourcedropdownlist = $('#cboSearchSourceActivity').data(""kendoDropDownList"");
        sourcedropdownlist.open();
        sourcedropdownlist.close();
    });
</script>
");

        }
    }
}
#pragma warning restore 1591