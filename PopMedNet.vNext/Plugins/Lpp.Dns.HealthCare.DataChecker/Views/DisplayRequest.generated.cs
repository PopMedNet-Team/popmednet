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

namespace Lpp.Dns.HealthCare.DataChecker.Views
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    
    #line 1 "..\..\Views\DisplayRequest.cshtml"
    using System.Reflection;
    
    #line default
    #line hidden
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
    using Lpp.Dns.HealthCare.Controllers;
    
    #line 4 "..\..\Views\DisplayRequest.cshtml"
    using Lpp.Dns.Portal;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Views\DisplayRequest.cshtml"
    using Lpp.Mvc;
    
    #line default
    #line hidden
    using Lpp.Mvc.Application;
    using Lpp.Mvc.Controls;
    
    #line 2 "..\..\Views\DisplayRequest.cshtml"
    using Newtonsoft.Json;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/DisplayRequest.cshtml")]
    public partial class DisplayRequest : System.Web.Mvc.WebViewPage<Lpp.Dns.HealthCare.DataChecker.Models.DataCheckerModel>
    {
        public DisplayRequest()
        {
        }
        public override void Execute()
        {
            
            #line 6 "..\..\Views\DisplayRequest.cshtml"
  
    Assembly assembly = typeof(Lpp.Dns.General.Metadata.MetadataSearchRequestType).Assembly;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n    var KOViewModel = true;     // indicates this page is built with KO templa" +
"tes\r\n</script>\r\n\r\n<div");

WriteLiteral(" id=\"CodesTerm\"");

WriteLiteral(" style=\"display: none;\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"label-groupbox\"");

WriteLiteral(">\r\n        <table>\r\n            <thead>\r\n                <tr>\r\n                  " +
"  <th");

WriteLiteral(" data-bind=\"visible: CodesTermType() == RequestCriteriaModels.CodesTermTypes.Diag" +
"nosis_ICD9Term\"");

WriteLiteral(">Diagnosis Codes</th>\r\n                    <th");

WriteLiteral(" data-bind=\"visible: CodesTermType() == RequestCriteriaModels.CodesTermTypes.Proc" +
"edure_ICD9Term\"");

WriteLiteral(">Procedure Codes</th>\r\n                    <th");

WriteLiteral(" data-bind=\"visible: CodesTermType() == RequestCriteriaModels.CodesTermTypes.NDCT" +
"erm\"");

WriteLiteral(">NDC Codes</th>\r\n                </tr>\r\n            </thead>\r\n            <tbody>" +
"\r\n                <tr>\r\n                    <td");

WriteLiteral(" data-bind=\"visible: CodesTermType() == RequestCriteriaModels.CodesTermTypes.Diag" +
"nosis_ICD9Term\"");

WriteLiteral(">\r\n                        <select");

WriteLiteral(" data-bind=\"value: CodeType\"");

WriteLiteral(" disabled>\r\n                            <option");

WriteLiteral(" value=\"\"");

WriteLiteral(">(Any)</option>\r\n                            <option");

WriteLiteral(" value=\"09\"");

WriteLiteral(">ICD-9-CM</option>\r\n                            <option");

WriteLiteral(" value=\"10\"");

WriteLiteral(">ICD-10-CM</option>\r\n                            <option");

WriteLiteral(" value=\"11\"");

WriteLiteral(">ICD-11-CM</option>\r\n                            <option");

WriteLiteral(" value=\"SM\"");

WriteLiteral(">SNOMED CT</option>\r\n                            <option");

WriteLiteral(" value=\"OT\"");

WriteLiteral(">Other</option>\r\n                        </select>\r\n                    </td>\r\n  " +
"                  <td");

WriteLiteral(" data-bind=\"visible: CodesTermType() == RequestCriteriaModels.CodesTermTypes.Proc" +
"edure_ICD9Term\"");

WriteLiteral(">\r\n                        <select");

WriteLiteral(" data-bind=\"value: CodeType\"");

WriteLiteral(" disabled>\r\n                            <option");

WriteLiteral(" value=\"\"");

WriteLiteral(">(Any)</option>\r\n                            <option");

WriteLiteral(" value=\"09\"");

WriteLiteral(">ICD-9-CM</option>\r\n                            <option");

WriteLiteral(" value=\"10\"");

WriteLiteral(">ICD-10-CM</option>\r\n                            <option");

WriteLiteral(" value=\"11\"");

WriteLiteral(">ICD-11-CM</option>\r\n                            <option");

WriteLiteral(" value=\"C2\"");

WriteLiteral(">CPT Category II</option>\r\n                            <option");

WriteLiteral(" value=\"C3\"");

WriteLiteral(">CPT Category III</option>\r\n                            <option");

WriteLiteral(" value=\"C4\"");

WriteLiteral(">CPT-4 (i.e., HCPCS Level I)</option>\r\n                            <option");

WriteLiteral(" value=\"HC\"");

WriteLiteral(">HCPCS (i.e., HCPCS Level II)</option>\r\n                            <option");

WriteLiteral(" value=\"H3\"");

WriteLiteral(">HCPCS Level III</option>\r\n                            <option");

WriteLiteral(" value=\"LC\"");

WriteLiteral(">LOINC</option>\r\n                            <option");

WriteLiteral(" value=\"LO\"");

WriteLiteral(">Local Homegrown</option>\r\n                            <option");

WriteLiteral(" value=\"ND\"");

WriteLiteral(">NDC</option>\r\n                            <option");

WriteLiteral(" value=\"RE\"");

WriteLiteral(">Revenue</option>\r\n                            <option");

WriteLiteral(" value=\"OT\"");

WriteLiteral(">Other</option>\r\n                        </select>\r\n                    </td>\r\n  " +
"                  <td");

WriteLiteral(" data-bind=\"visible: CodesTermType() == RequestCriteriaModels.CodesTermTypes.NDCT" +
"erm\"");

WriteLiteral(">\r\n                        &nbsp;\r\n                    </td>\r\n                </t" +
"r>\r\n\r\n                <tr>\r\n                    <td>\r\n                        <t" +
"extarea");

WriteLiteral(" style=\"height: 70px; width: 95%;\"");

WriteLiteral(" data-bind=\"value: Codes\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral("></textarea><br />\r\n                        <input");

WriteLiteral(" type=\"radio\"");

WriteLiteral(" name=\"SearchTermMethod\"");

WriteLiteral(" data-bind=\"value: RequestCriteriaModels.SearchMethodTypes.ExactMatch, checked: S" +
"earchMethodType\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" /><span>Exact Match</span><br />\r\n                        <input");

WriteLiteral(" type=\"radio\"");

WriteLiteral(" name=\"SearchTermMethod\"");

WriteLiteral(" data-bind=\"value: RequestCriteriaModels.SearchMethodTypes.StartsWith, checked: S" +
"earchMethodType\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" /><span>Starts With</span><br />\r\n                    </td>\r\n                </t" +
"r>\r\n            </tbody>\r\n        </table>\r\n    </div>\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"DataPartnerTerm\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral(">\r\n    <table>\r\n        <thead>\r\n            <tr>\r\n                <th");

WriteLiteral(" style=\"width: 24px;\"");

WriteLiteral(">\r\n\r\n                </th>\r\n                <th>Data Partner</th>\r\n            </" +
"tr>\r\n        </thead>\r\n        <tbody>\r\n            <tr>\r\n                <td></" +
"td>\r\n                <td></td>\r\n            </tr>\r\n        </tbody>\r\n        <tb" +
"ody");

WriteLiteral(" id=\"DataPartnersTable\"");

WriteLiteral(" data-bind=\"foreach: ");

            
            #line 87 "..\..\Views\DisplayRequest.cshtml"
                                                      Write(Newtonsoft.Json.JsonConvert.SerializeObject(Model.DataPartners));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n            <tr>\r\n                <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"value: Value, checked: $parent.DataPartners\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                </td>\r\n                <td>\r\n                    <span");

WriteLiteral(" data-bind=\"text: Key\"");

WriteLiteral("></span>&nbsp;(<span");

WriteLiteral(" data-bind=\"text: Value\"");

WriteLiteral("> </span>)\r\n                </td>\r\n            </tr>\r\n        </tbody>\r\n    </tab" +
"le>\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"EthnicityTerm\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral(">\r\n    <table>\r\n        <thead>\r\n            <tr>\r\n                <th");

WriteLiteral(" style=\"width: 24px;\"");

WriteLiteral(">\r\n\r\n                </th>\r\n                <th>Ethnicity</th>\r\n            </tr>" +
"\r\n        </thead>\r\n        <tbody");

WriteLiteral(" id=\"EthnicityTable\"");

WriteLiteral(" data-bind=\"foreach: DataCheckerViewModels.EthnicityTerm.EthnicitiesList\"");

WriteLiteral(">\r\n            <tr>\r\n                <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"value: Value, checked: $parent.Ethnicities\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                </td>\r\n                <td>\r\n                    <span");

WriteLiteral(" data-bind=\"text: Key\"");

WriteLiteral("></span>\r\n                </td>\r\n            </tr>\r\n        </tbody>\r\n    </table" +
">\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"MetricTerm\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral(">\r\n    <table>\r\n        <thead>\r\n            <tr>\r\n                <th");

WriteLiteral(" style=\"width: 24px;\"");

WriteLiteral(">\r\n\r\n                </th>\r\n                <th>Metric</th>\r\n            </tr>\r\n " +
"       </thead>\r\n        <tbody");

WriteLiteral(" id=\"MetricsTable\"");

WriteLiteral(" data-bind=\"foreach: MetricsList\"");

WriteLiteral(">\r\n            <tr>\r\n                <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"value: Value, checked: $parent.Metrics\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                </td>\r\n                <td>\r\n                    <span");

WriteLiteral(" data-bind=\"text: Key\"");

WriteLiteral("></span>\r\n                </td>\r\n            </tr>\r\n        </tbody>\r\n    </table" +
">\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"RaceTerm\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral(">\r\n    <table>\r\n        <thead>\r\n            <tr>\r\n                <th");

WriteLiteral(" style=\"width: 24px;\"");

WriteLiteral(">\r\n\r\n                </th>\r\n                <th>Race</th>\r\n            </tr>\r\n   " +
"     </thead>\r\n        <tbody");

WriteLiteral(" id=\"RaceTable\"");

WriteLiteral(" data-bind=\"foreach: DataCheckerViewModels.RaceTerm.RacesList\"");

WriteLiteral(">\r\n            <tr>\r\n                <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"value: Value, checked: $parent.Races\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                </td>\r\n                <td>\r\n                    <span");

WriteLiteral(" data-bind=\"text: Key\"");

WriteLiteral("></span>\r\n                </td>\r\n            </tr>\r\n        </tbody>\r\n    </table" +
">\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"RxSupTerm\"");

WriteLiteral(" style=\"display:none;\"");

WriteLiteral(">\r\n    <table>\r\n        <thead>\r\n            <tr>\r\n                <th>RxSup</th>" +
"\r\n            </tr>\r\n        </thead>\r\n        <tbody>\r\n\r\n            <tr>\r\n    " +
"            <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: RxSups\"");

WriteLiteral(" value=\"0\"");

WriteLiteral(" id=\"rxLessThanZero\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"rxLessThanZero\"");

WriteLiteral(">&lt;0</label>\r\n                </td>\r\n            </tr>\r\n\r\n            <tr>\r\n   " +
"             <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: RxSups\"");

WriteLiteral(" value=\"1\"");

WriteLiteral(" id=\"rxSupZero\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"rxSupZero\"");

WriteLiteral(">0-1</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n       " +
"         <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: RxSups\"");

WriteLiteral(" value=\"2\"");

WriteLiteral(" id=\"rxSupTwo\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"rxSupTwo\"");

WriteLiteral(">2-30</label>\r\n                </td>\r\n            </tr>\r\n\r\n            <tr>\r\n    " +
"            <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: RxSups\"");

WriteLiteral(" value=\"3\"");

WriteLiteral(" id=\"rxSupThirty\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"rxSupThirty\"");

WriteLiteral(">31-60</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n     " +
"           <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: RxSups\"");

WriteLiteral(" value=\"4\"");

WriteLiteral(" id=\"rxSupSixty\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"rxSupSixty\"");

WriteLiteral(">61-90</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n     " +
"           <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: RxSups\"");

WriteLiteral(" value=\"5\"");

WriteLiteral(" id=\"rxSupNinety\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"rxSupNinety\"");

WriteLiteral(">&gt;90</label>\r\n                </td>\r\n            </tr>\r\n            ");

WriteLiteral("\r\n            <tr>\r\n                <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: RxSups\"");

WriteLiteral(" value=\"7\"");

WriteLiteral(" id=\"rxSupMissing\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"rxSupMissing\"");

WriteLiteral(">Missing</label>\r\n                </td>\r\n            </tr>\r\n\r\n        </tbody>\r\n " +
"   </table>\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"RxAmtTerm\"");

WriteLiteral(" style=\"display:none;\"");

WriteLiteral(">\r\n    <table>\r\n        <thead>\r\n            <tr>\r\n                <th>RxAmt</th>" +
"\r\n            </tr>\r\n        </thead>\r\n        <tbody>\r\n            <tr>\r\n      " +
"          <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: RxAmounts\"");

WriteLiteral(" value=\"0\"");

WriteLiteral(" id=\"rxAmtLessThanZero\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"rxAmtLessThanZero\"");

WriteLiteral(">&lt;0</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n     " +
"           <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: RxAmounts\"");

WriteLiteral(" value=\"1\"");

WriteLiteral(" id=\"rxAmtZero\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"rxAmtZero\"");

WriteLiteral(">0-1</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n       " +
"         <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: RxAmounts\"");

WriteLiteral(" value=\"2\"");

WriteLiteral(" id=\"rxAmtTwoThrough30\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"rxAmtTwoThrough30\"");

WriteLiteral(">2-30</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n      " +
"          <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: RxAmounts\"");

WriteLiteral(" value=\"3\"");

WriteLiteral(" id=\"rxAmtThirty\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"rxAmtThirty\"");

WriteLiteral(">31-60</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n     " +
"           <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: RxAmounts\"");

WriteLiteral(" value=\"4\"");

WriteLiteral(" id=\"rxAmtSixty\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"rxAmtSixty\"");

WriteLiteral(">61-90</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n     " +
"           <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: RxAmounts\"");

WriteLiteral(" value=\"5\"");

WriteLiteral(" id=\"rxAmtNinety\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"rxAmtNinety\"");

WriteLiteral(">91-120</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n    " +
"            <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: RxAmounts\"");

WriteLiteral(" value=\"6\"");

WriteLiteral(" id=\"rxAmtOneTwenty\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"rxAmtOneTwenty\"");

WriteLiteral(">121-180</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n   " +
"             <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: RxAmounts\"");

WriteLiteral(" value=\"7\"");

WriteLiteral(" id=\"rxAmtOne180\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"rxAmtOne180\"");

WriteLiteral(">&gt;180</label>\r\n                </td>\r\n            </tr>\r\n\r\n            ");

WriteLiteral("\r\n            <tr>\r\n                <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: RxAmounts\"");

WriteLiteral(" value=\"9\"");

WriteLiteral(" id=\"rxAmtMissing\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"rxAmtMissing\"");

WriteLiteral(">Missing</label>\r\n                </td>\r\n            </tr>\r\n\r\n        </tbody>\r\n " +
"   </table>\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"PDXTerm\"");

WriteLiteral(" style=\"display:none;\"");

WriteLiteral(">\r\n    <table>\r\n        <thead>\r\n            <tr>\r\n                <th>PDX</th>\r\n" +
"            </tr>\r\n        </thead>\r\n        <tbody>\r\n            <tr>\r\n        " +
"        <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: PDXes\"");

WriteLiteral(" value=\"0\"");

WriteLiteral(" id=\"pdxPrinciple\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"pdxPrinciple\"");

WriteLiteral(">Principle</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n " +
"               <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: PDXes\"");

WriteLiteral(" value=\"1\"");

WriteLiteral(" id=\"pdxSecondary\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"pdxSecondary\"");

WriteLiteral(">Secondary</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n " +
"               <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: PDXes\"");

WriteLiteral(" value=\"2\"");

WriteLiteral(" id=\"pdxOther\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"pdxOther\"");

WriteLiteral(">Other PDX</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n " +
"               <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: PDXes\"");

WriteLiteral(" value=\"3\"");

WriteLiteral(" id=\"pdxMissing\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"pdxMissing\"");

WriteLiteral(">Missing</label>\r\n                </td>\r\n            </tr>\r\n        </tbody>\r\n   " +
" </table>\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"EncounterTypeTerm\"");

WriteLiteral(" style=\"display:none;\"");

WriteLiteral(">\r\n    <table>\r\n        <thead>\r\n            <tr>\r\n                <th>Encounter " +
"Type</th>\r\n            </tr>\r\n        </thead>\r\n        <tbody>\r\n            <tr" +
">\r\n                <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: Encounters\"");

WriteLiteral(" value=\"0\"");

WriteLiteral(" id=\"encounterAll\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"encounterAll\"");

WriteLiteral(">All</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n       " +
"         <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: Encounters\"");

WriteLiteral(" value=\"1\"");

WriteLiteral(" id=\"encounterAmbulatoryVisit\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"encounterAmbulatoryVisit\"");

WriteLiteral(">Ambulatory Visit (AV)</label>\r\n                </td>\r\n            </tr>\r\n       " +
"     <tr>\r\n                <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: Encounters\"");

WriteLiteral(" value=\"2\"");

WriteLiteral(" id=\"encounterEmergencyDepartment\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"encounterEmergencyDepartment\"");

WriteLiteral(">Emergency Department (ED)</label>\r\n                </td>\r\n            </tr>\r\n   " +
"         <tr>\r\n                <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: Encounters\"");

WriteLiteral(" value=\"3\"");

WriteLiteral(" id=\"encounterInpatientHospitalStay\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"encounterInpatientHospitalStay\"");

WriteLiteral(">Inpatient Hospital Stay (IP)</label>\r\n                </td>\r\n            </tr>\r\n" +
"            <tr>\r\n                <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: Encounters\"");

WriteLiteral(" value=\"4\"");

WriteLiteral(" id=\"encounterNonAcuteInstitutionalStay\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"encounterNonAcuteInstitutionalStay\"");

WriteLiteral(">Non-Acute Institutional Stay (IS)</label>\r\n                </td>\r\n            </" +
"tr>\r\n            <tr>\r\n                <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: Encounters\"");

WriteLiteral(" value=\"5\"");

WriteLiteral(" id=\"encounterOther\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"encounterOther\"");

WriteLiteral(">Other Ambulatory Visit (OA)</label>\r\n                </td>\r\n            </tr>\r\n " +
"           <tr>\r\n                <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: Encounters\"");

WriteLiteral(" value=\"6\"");

WriteLiteral(" id=\"encounterMissing\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"encounterMissing\"");

WriteLiteral(">Missing</label>\r\n                </td>\r\n            </tr>\r\n\r\n        </tbody>\r\n " +
"   </table>\r\n\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"MetaDataTableTerm\"");

WriteLiteral(" style=\"display:none;\"");

WriteLiteral(">\r\n    <table>\r\n        <thead>\r\n            <tr>\r\n                <th>Table</th>" +
"\r\n            </tr>\r\n        </thead>\r\n        <tbody>\r\n            <tr>\r\n      " +
"          <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: Tables\"");

WriteLiteral(" value=\"0\"");

WriteLiteral(" id=\"tableDiagnosis\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"tableDiagnosis\"");

WriteLiteral(">Diagnosis</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n " +
"               <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: Tables\"");

WriteLiteral(" value=\"1\"");

WriteLiteral(" id=\"tableDispensing\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"tableDispensing\"");

WriteLiteral(">Dispensing</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n" +
"                <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: Tables\"");

WriteLiteral(" value=\"2\"");

WriteLiteral(" id=\"tableEncounter\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"tableEncounter\"");

WriteLiteral(">Encounter</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n " +
"               <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: Tables\"");

WriteLiteral(" value=\"3\"");

WriteLiteral(" id=\"tableEnrollment\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"tableEnrollment\"");

WriteLiteral(">Enrollment</label>\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n" +
"                <td>\r\n                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: Tables\"");

WriteLiteral(" value=\"4\"");

WriteLiteral(" id=\"tabelProcedure\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(" />\r\n                    <label");

WriteLiteral(" for=\"tabelProcedure\"");

WriteLiteral(">Procedure</label>\r\n                </td>\r\n            </tr>\r\n\r\n        </tbody>\r" +
"\n    </table>\r\n</div>\r\n\r\n\r\n<div");

WriteLiteral(" class=\"DataChecker ui-form\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"ui-form\"");

WriteLiteral(">\r\n        <fieldset");

WriteLiteral(" id=\"fsCriteria\"");

WriteLiteral(">\r\n            <legend");

WriteLiteral(" style=\"display: none;\"");

WriteLiteral("></legend>\r\n");

WriteLiteral("            ");

            
            #line 447 "..\..\Views\DisplayRequest.cshtml"
       Write(Html.HiddenFor(m => m.CriteriaGroupsJSON));

            
            #line default
            #line hidden
WriteLiteral("\r\n            <div");

WriteLiteral(" id=\'errorLocation\'");

WriteLiteral(" style=\"font-size: small; color: Gray;\"");

WriteLiteral("></div>\r\n            <div");

WriteLiteral(" class=\"ui-groupbox\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"ui-groupbox-header\"");

WriteLiteral(">\r\n                    <span>Data Checker Criteria</span>\r\n                </div>" +
"\r\n                <ol");

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

WriteLiteral(" type=\"text/javascript\"");

WriteAttribute("src", Tuple.Create(" src=\"", 18317), Tuple.Create("\"", 18382)
            
            #line 468 "..\..\Views\DisplayRequest.cshtml"
, Tuple.Create(Tuple.Create("", 18323), Tuple.Create<System.Object, System.Int32>(this.Url.Resource(assembly, "Models/DataCheckerModels.js")
            
            #line default
            #line hidden
, 18323), false)
);

WriteLiteral("></script>\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteAttribute("src", Tuple.Create(" src=\"", 18425), Tuple.Create("\"", 18494)
            
            #line 469 "..\..\Views\DisplayRequest.cshtml"
, Tuple.Create(Tuple.Create("", 18431), Tuple.Create<System.Object, System.Int32>(this.Url.Resource(assembly, "Models/RequestCriteriaModels.js")
            
            #line default
            #line hidden
, 18431), false)
);

WriteLiteral("></script>\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteAttribute("src", Tuple.Create(" src=\"", 18537), Tuple.Create("\"", 18610)
            
            #line 470 "..\..\Views\DisplayRequest.cshtml"
, Tuple.Create(Tuple.Create("", 18543), Tuple.Create<System.Object, System.Int32>(this.Url.Resource(assembly, "ViewModels/DataCheckerViewModels.js")
            
            #line default
            #line hidden
, 18543), false)
);

WriteLiteral("></script>\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteAttribute("src", Tuple.Create(" src=\"", 18653), Tuple.Create("\"", 18730)
            
            #line 471 "..\..\Views\DisplayRequest.cshtml"
, Tuple.Create(Tuple.Create("", 18659), Tuple.Create<System.Object, System.Int32>(this.Url.Resource(assembly, "ViewModels/RequestCriteriaViewModels.js")
            
            #line default
            #line hidden
, 18659), false)
);

WriteLiteral("></script>\r\n\r\n");

WriteLiteral("\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteAttribute("src", Tuple.Create(" src=\"", 18799), Tuple.Create("\"", 18843)
            
            #line 474 "..\..\Views\DisplayRequest.cshtml"
, Tuple.Create(Tuple.Create("", 18805), Tuple.Create<System.Object, System.Int32>(this.Resource("ViewModels/Create.js")
            
            #line default
            #line hidden
, 18805), false)
);

WriteLiteral("></script>\r\n\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n    jQuery(document).ready(function () {\r\n        // initialize the viewmodel\r" +
"\n        //displayrequest.cshtml\r\n        var json = ");

            
            #line 480 "..\..\Views\DisplayRequest.cshtml"
               Write(Html.Raw(HttpUtility.HtmlDecode(Model.CriteriaGroupsJSON) + ";"));

            
            #line default
            #line hidden
WriteLiteral("\r\n        DataChecker.Create.init(json, $(\"#fsCriteria\"), $(\"#CriteriaGroupsJSON\"" +
"));\r\n    });\r\n</script>");

        }
    }
}
#pragma warning restore 1591