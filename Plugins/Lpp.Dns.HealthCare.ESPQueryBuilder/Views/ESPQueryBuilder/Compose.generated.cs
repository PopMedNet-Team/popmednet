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

namespace Lpp.Dns.HealthCare.ESPQueryBuilder.Views.ESPQueryBuilder
{
    using System;
    
    #line 9 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
    using System.Collections.Generic;
    
    #line default
    #line hidden
    using System.IO;
    
    #line 10 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
    using System.Linq;
    
    #line default
    #line hidden
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    
    #line 11 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
    using System.Web.Mvc;
    
    #line default
    #line hidden
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Lpp;
    
    #line 2 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
    using Lpp.Dns.General.CriteriaGroup.Models;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
    using Lpp.Dns.General.CriteriaGroup.Views.CriteriaGroup;
    
    #line default
    #line hidden
    
    #line 4 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
    using Lpp.Dns.General.CriteriaGroup.Views.CriteriaGroup.Terms;
    
    #line default
    #line hidden
    using Lpp.Dns.HealthCare.Controllers;
    using Lpp.Dns.HealthCare.ESPQueryBuilder;
    
    #line 5 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
    using Lpp.Dns.HealthCare.ESPQueryBuilder.Data;
    
    #line default
    #line hidden
    using Lpp.Dns.HealthCare.ESPQueryBuilder.Models;
    using Lpp.Dns.HealthCare.ESPQueryBuilder.Views;
    
    #line 6 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
    using Lpp.Dns.HealthCare.ESPQueryBuilder.Views.ESPQueryBuilder;
    
    #line default
    #line hidden
    
    #line 7 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
    using Lpp.Dns.HealthCare.ESPQueryBuilder.Views.ESPQueryBuilder.Terms;
    
    #line default
    #line hidden
    using Lpp.Mvc;
    using Lpp.Mvc.Application;
    using Lpp.Mvc.Controls;
    
    #line 8 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
    using Lpp.Utilities;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ESPQueryBuilder/Compose.cshtml")]
    public partial class Compose : System.Web.Mvc.WebViewPage<Lpp.Dns.HealthCare.ESPQueryBuilder.Models.ESPQueryBuilderModel>
    {
        public Compose()
        {
        }
        public override void Execute()
        {
            
            #line 12 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
   this.Stylesheet("ESPQueryComposition.min.css"); 
            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 13 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
  
    var criteriaGroupTipText = "Click the [+] in the upper right corner to add Criteria Groups. Criteria Groups form cohorts that are determined by the terms within each group. ICD-9 Diagnosis and Disease terms within a criteria groups are OR-ed, whereas Demographic terms (Sex, Race, Ethnicity, Age, Zip Codes) and Visit terms are AND-ed. Multiple Criteria Groups are AND-ed to determine the final cohort which will display a single or stratified patient count based on the Result Stratification options selected. You may negate a Criteria Group by checking 'exclude criteria group' box which will filter out that groups cohort from the final cohort. For more information, see <a href='https://popmednet.atlassian.net/wiki/display/DOC/Query+Composer+Request' target='_blank'>PopMedNet User's Guide: Query Composer Request</a>";

    var reportsModel = new {
        Report = (from s in (Model.Report ?? string.Empty).TrimStart(',').Split(',')
                  where !string.IsNullOrWhiteSpace(s) && System.Text.RegularExpressions.Regex.IsMatch(s, @"\d?", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline)
                  select Convert.ToInt32(s)).ToArray(),
        ReportSelections = Model.ReportSelections,
        Model.AgeStratification,
        Model.PeriodStratification,
        Model.ICD9Stratification
    };
    

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" id=\"ESPCompose\"");

WriteLiteral(" class=\"MedicalRequest ui-form\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"ui-form\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 29 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
   Write(Html.HiddenFor(m => m.CriteriaGroupsJSON));

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div");

WriteLiteral(" id=\"CriteriaGroups\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"panel panel-default\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"panel-heading\"");

WriteLiteral(">\r\n                    <button");

WriteLiteral(" class=\"btn btn-default pull-right\"");

WriteLiteral(" id=\"btnAddGroup\"");

WriteLiteral(" type=\"button\"");

WriteLiteral(" style=\"font-size:0.8em;position:relative;top:-5px;\"");

WriteLiteral("><span");

WriteLiteral(" class=\"glyphicon glyphicon-plus\"");

WriteLiteral("></span> Add Group</button>\r\n                    Criteria Groups\r\n               " +
"     <img");

WriteLiteral(" src=\"/Content/img/icons/help16.gif\"");

WriteLiteral(" class=\"helptip\"");

WriteAttribute("title", Tuple.Create(" title=\"", 2582), Tuple.Create("\"", 2621)
            
            #line 35 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
    , Tuple.Create(Tuple.Create("", 2590), Tuple.Create<System.Object, System.Int32>(Html.Raw(criteriaGroupTipText)
            
            #line default
            #line hidden
, 2590), false)
);

WriteLiteral(" />\r\n                    \r\n                </div>\r\n                <div");

WriteLiteral(" id=\"CriteriaGroupsContainer\"");

WriteLiteral(" class=\"panel-body\"");

WriteLiteral(">\r\n\r\n");

WriteLiteral("                ");

            
            #line 40 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
            Write(Html.Partial<CriteriaGroup>().WithModel(new CriteriaGroupModel { CriteriaGroupId = 0, ExcludeCriteriaGroup = false, SaveCriteriaGroup = true, Hidden = true, TermSelections = Model.TermSelections }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 42 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
                
            
            #line default
            #line hidden
            
            #line 42 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
                 if (Model.CriteriaGroups.Count() == 0) {
                    Model.CriteriaGroupsJSON = "[{\"CriteriaGroupName\":\"Primary\",\"ExcludeCriteriaGroup\":false,\"SaveCriteriaGroup\":true,\"Hidden\":false,\"Terms\":[]}]"; 
                }

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 46 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
                
            
            #line default
            #line hidden
            
            #line 46 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
                   int criteriaGroupId = 0; 
            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 47 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
                
            
            #line default
            #line hidden
            
            #line 47 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
                 foreach (var criteriaGroup in Model.CriteriaGroups)
                {
                    criteriaGroup.CriteriaGroupId = ++criteriaGroupId;
                    criteriaGroup.Hidden = false;
                    criteriaGroup.TermSelections = Model.TermSelections;


            
            #line default
            #line hidden
WriteLiteral("                    <div");

WriteLiteral(" class=\"CriteriaGroupOuter\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 54 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
                    Write(Html.Partial<CriteriaGroup>().WithModel(criteriaGroup));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n                        <div");

WriteLiteral(" class=\"CriteriaTerms\"");

WriteLiteral(">\r\n");

            
            #line 57 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
                            
            
            #line default
            #line hidden
            
            #line 57 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
                             foreach (var term in criteriaGroup.Terms)
                            {
                                switch (term.TermName)
                                {
                                    case "AgeRange":
                                        Model.MinAge = term.Args["MinAge"];
                                        Model.MaxAge = term.Args["MaxAge"];

                                        Write(Html.Partial<AgeRange>());
                                        break;
                                    case "DiseaseSelector":
                                        Model.Disease = term.Args["Disease"];
                                        Write(Html.Partial<DiseaseSelector>().WithModel(Model));
                                        break;
                                    case "Gender":
                                        var sex = 0;
                                        Int32.TryParse(term.Args["Sex"], out sex);
                                        Model.Sex = sex;
                                        Write(Html.Partial<Gender>().WithModel(Model));
                                        break;
                                    case "ICD9CodeSelector":
                                        Model.Codes = term.Args["Codes"];
                                        Write(Html.Partial<ICD9CodeSelector>().WithModel(Model));
                                        break;
                                    case "RaceSelector":
                                        Model.Race = term.Args["Race"];
                                        Write(Html.Partial<RaceSelector>().WithModel(Model));
                                        break;
                                    case "SmokingSelector":
                                        Model.Smoking = term.Args["Smoking"];
                                        Write(Html.Partial<SmokingSelector>().WithModel(Model));
                                        break;
                                    case "EthnicitySelector":
                                        Model.Race = term.Args["Ethnicity"];
                                        Write(Html.Partial<EthnicitySelector>().WithModel(Model));
                                        break;
                                    case "ZipCodeSelector":
                                        Model.ZipCodes = term.Args["Codes"];
                                        Write(Html.Partial<ZipCodeSelector>().WithModel(Model));
                                        break;
                                    case "ObservationPeriod":
                                        // Skip - handled by CriteriaGroup project.
                                        break;
                                    case "Visits":
                                        var visits = 0;
                                        Int32.TryParse(term.Args["MinVisits"], out visits);
                                        Model.MinVisits = visits;
                                        Write(Html.Partial<Visits>());
                                        break;
                                    case "CustomLocation":
                                        /*need to pull the term custom values and set on the model so that they can be loaded by the partial*/
                                        string clv;
                                        if (term.Args.TryGetValue("LocationName", out clv))
                                        {
                                            Model.LocationNames = clv;
                                        }
                                        if (term.Args.TryGetValue("LocationCodes", out clv))
                                        {
                                            Model.LocationCodes = clv;
                                        }
                                        Write(Html.Partial<CustomLocation>().WithModel(Model));
                                        break;
                                    case "PredefinedLocation":
                                        string selectedLocations;
                                        if (term.Args.TryGetValue("PredefinedLocations", out selectedLocations))
                                        {
                                            Model.LocationCodes = selectedLocations;
                                        }
                                        Write(Html.Partial<PredefinedLocation>().WithModel(Model));                                        
                                        break;
                                    default:
                                        Write(Html.Partial<Lpp.Dns.HealthCare.ESPQueryBuilder.Views.ESPQueryBuilder.Terms.Dummy>().WithoutModel());
                                        break;
                                }
                            }

            
            #line default
            #line hidden
WriteLiteral("                        </div>\r\n                    </div>\r\n");

            
            #line 134 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("                </div>\r\n            </div>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"ReportParameters\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"panel panel-default\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"panel-heading\"");

WriteLiteral(">\r\n                    Result Stratification<img");

WriteLiteral(" src=\"/Content/img/icons/help16.gif\"");

WriteLiteral(" class=\"helptip\"");

WriteLiteral(" title=\"Results are stratified according to the criteria in the Primary Criteria " +
"Group. If no option is selected, a single patient count will be returned. \"");

WriteLiteral(" />\r\n                </div>\r\n                <div");

WriteLiteral(" class=\"panel-body Selections\"");

WriteLiteral(">\r\n                    <fieldset");

WriteLiteral(" id=\"reports-container\"");

WriteLiteral(">\r\n                        <table");

WriteLiteral(" class=\"table table-striped\"");

WriteLiteral(">\r\n                            <thead>\r\n                            <tr>\r\n       " +
"                         <th");

WriteLiteral(" class=\"checkbox-col\"");

WriteLiteral("></th>\r\n                                <th>Variable</th>\r\n                      " +
"          <th");

WriteLiteral(" class=\"stratification-setting-col\"");

WriteLiteral(">Setting</th>\r\n                            </tr>\r\n                            </t" +
"head>\r\n                            <tbody");

WriteLiteral(" data-bind=\"foreach: ReportSelections\"");

WriteLiteral(">\r\n                                <tr");

WriteLiteral(" data-bind=\"attr:{ id:Value}\"");

WriteLiteral(">\r\n                                    <td");

WriteLiteral(" class=\"checkbox-col\"");

WriteLiteral(">\r\n                                        <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"value:Value, checked:$root.Report\"");

WriteLiteral(" />\r\n                                    </td>\r\n                                 " +
"   <td");

WriteLiteral(" data-bind=\"text:Display\"");

WriteLiteral("></td>\r\n                                    <td>\r\n                               " +
"         <!-- ko if:SelectionList != null && SelectionList.length > 0 -->\r\n     " +
"                                   <input");

WriteLiteral(" data-bind=\"kendoDropDownList:{data:SelectionList, dataTextField:\'CategoryText\', " +
"dataValueField:\'StratificationCategoryId\', value:$root.GetReportSettingProperty(" +
"Name)}\"");

WriteLiteral(" />\r\n                                        <!-- /ko -->\r\n                      " +
"              </td>\r\n                                </tr>\r\n                    " +
"        </tbody>\r\n                        </table>\r\n\r\n                        <i" +
"nput");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" name=\"Report\"");

WriteLiteral(" data-bind=\"value:Report\"");

WriteLiteral("/>\r\n                        <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" name=\"AgeStratification\"");

WriteLiteral(" data-bind=\"value:AgeStratification\"");

WriteLiteral(" />\r\n                        <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" name=\"PeriodStratification\"");

WriteLiteral(" data-bind=\"value:PeriodStratification\"");

WriteLiteral(" />\r\n                        <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" name=\"ICD9Stratification\"");

WriteLiteral(" data-bind=\"value:ICD9Stratification\"");

WriteLiteral(" />\r\n                    </fieldset>\r\n                </div>\r\n            </div>\r" +
"\n        </div>\r\n    </div>\r\n</div>\r\n<script");

WriteAttribute("src", Tuple.Create(" src=\"", 11414), Tuple.Create("\"", 11448)
            
            #line 178 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
, Tuple.Create(Tuple.Create("", 11420), Tuple.Create<System.Object, System.Int32>(this.Resource("compose.js")
            
            #line default
            #line hidden
, 11420), false)
);

WriteLiteral("></script>\r\n<script>\r\n    config.reportcodes = {\r\n        age: ");

            
            #line 181 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
        Write(ReportSelectionCode.Age.ToString("d"));

            
            #line default
            #line hidden
WriteLiteral(",\r\n        center: ");

            
            #line 182 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
           Write(ReportSelectionCode.Center.ToString("d"));

            
            #line default
            #line hidden
WriteLiteral(",\r\n        disease: ");

            
            #line 183 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
            Write(ReportSelectionCode.Disease.ToString("d"));

            
            #line default
            #line hidden
WriteLiteral(",\r\n        icd9: ");

            
            #line 184 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
         Write(ReportSelectionCode.ICD9.ToString("d"));

            
            #line default
            #line hidden
WriteLiteral(",\r\n        period: ");

            
            #line 185 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
           Write(ReportSelectionCode.Period.ToString("d"));

            
            #line default
            #line hidden
WriteLiteral(",\r\n        race: ");

            
            #line 186 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
         Write(ReportSelectionCode.Race.ToString("d"));

            
            #line default
            #line hidden
WriteLiteral(",\r\n        ethnicity: ");

            
            #line 187 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
              Write(ReportSelectionCode.Ethnicity.ToString("d"));

            
            #line default
            #line hidden
WriteLiteral(",\r\n        sex: ");

            
            #line 188 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
        Write(ReportSelectionCode.Sex.ToString("d"));

            
            #line default
            #line hidden
WriteLiteral(",\r\n        zip: ");

            
            #line 189 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
        Write(ReportSelectionCode.Zip.ToString("d"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    };\r\n    config.termSelectionUrl = \'");

            
            #line 191 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
                          Write(Model.TermSelections.Url);

            
            #line default
            #line hidden
WriteLiteral("\';\r\n\r\n    reportsModel = ");

            
            #line 193 "..\..\Views\ESPQueryBuilder\Compose.cshtml"
              Write(Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(reportsModel)));

            
            #line default
            #line hidden
WriteLiteral(";\r\n\r\n    initCompose();\r\n</script>");

        }
    }
}
#pragma warning restore 1591
