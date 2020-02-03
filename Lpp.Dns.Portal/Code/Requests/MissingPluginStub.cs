using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace Lpp.Dns.Portal
{
    internal class MissingPluginStub : IDnsModelPlugin
    {
        public static readonly MissingPluginStub Instance = new MissingPluginStub();

        public const string MissingMessage = "The plugin that was used to create this request is missing or corrupted.";
        public static readonly IDnsModel Model;
        public static readonly IDnsRequestType Request;
        static IHtmlString ReturnMissingMessage(HtmlHelper _) { return new MvcHtmlString(MissingMessage); }

        static MissingPluginStub()
        {
            Request = Dns.RequestType("{2C341D3B-9A02-49A1-9E26-0A00550C9ABA}", "N/A", "");
            Model = Dns.Model("{8CB1AF3B-EBEB-44CC-A935-8364545EA0D0}", Guid.Empty.ToString(), "N/A", Request);
        }

        private MissingPluginStub() { }

        public string Version
        {
            get
            {
                return System.Diagnostics.FileVersionInfo.GetVersionInfo(this.GetType().Assembly.Location).FileVersion;
            }
        }

        public IEnumerable<IDnsModel> Models 
        { 
            get 
            { 
                return new[] { Model }; 
            } 
        }

        public Func<HtmlHelper, IHtmlString> DisplayRequest(IDnsRequestContext context) { return ReturnMissingMessage; }
        
        public Func<HtmlHelper, IHtmlString> DisplayConfigurationForm(IDnsModel model, Dictionary<string, string> properties) { return null; }
        
        public IEnumerable<string> ValidateConfig(ArrayList config) { return null; }
        
        public Func<HtmlHelper, IHtmlString> DisplayResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode) { return ReturnMissingMessage; }
        
        public IEnumerable<IDnsResponseExportFormat> GetExportFormats(IDnsResponseContext context) { return null; }
        
        public IDnsDocument ExportResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format, string args) { return null; }
        
        public Func<HtmlHelper, IHtmlString> EditRequestView(IDnsRequestContext context) { return ReturnMissingMessage; }
        
        public Func<HtmlHelper, IHtmlString> EditRequestReDisplay(IDnsRequestContext request, IDnsPostContext post) { return ReturnMissingMessage; }
        
        public DnsRequestTransaction EditRequestPost(IDnsRequestContext request, IDnsPostContext post) { return DnsRequestTransaction.Failed(MissingMessage); }
        
        public void CacheMetadataResponse(Guid requestID, IDnsDataMartResponse response) { }
        
        public DnsResult ValidateForSubmission(IDnsRequestContext context) { return DnsResult.Success; }
        
        public IEnumerable<IDnsResponseAggregationMode> GetAggregationModes(IDnsRequestContext context) { return null; }
        
        public DnsRequestTransaction TimeShift(IDnsRequestContext ctx, TimeSpan timeDifference) { return new DnsRequestTransaction(); }
        
        public DnsResponseTransaction ExecuteRequest(IDnsRequestContext context) { throw new NotImplementedException(); }
    }
}