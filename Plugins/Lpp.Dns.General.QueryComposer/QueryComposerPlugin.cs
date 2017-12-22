using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using Lpp.Composition;
//using Lpp.Data;
using Lpp.Dns.DocumentVisualizers.Views;
using Lpp.Mvc;
using Lpp.Dns.Portal;
using Lpp.Security;
using Lpp.Dns.Model;
using Lpp.Dns.General;
using Lpp.Dns;

namespace Lpp.Dns.General.QueryComposer
{
    public class QueryComposerRequestType : IDnsRequestType
    {
        public static IEnumerable<QueryComposerRequestType> All { get { return RequestTypes; } }

        public const string ModelProcessorId = "{AE0DA7B0-0F73-4D06-B70B-922032B7F0EB}";

        public static readonly IEnumerable<QueryComposerRequestType> RequestTypes = new[]
        {
            new  QueryComposerRequestType( "{A3044773-8387-4C1B-8139-92B281D0467C}", "Test Query Composer", "", "Test Query Composer")
        };


        public Guid ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get { return _description; } }
        public string ShortDescription { get; private set; }
        public bool IsMetadataRequest { get { return false; } }

        public QueryComposerRequestType(string id, string name, string description, string shortDescription)
        {
            ID = new Guid(id);
            Name = name;
            ShortDescription = shortDescription;
        }

        private const string _description = @"This page allows you to INSERT LONG DESCRIPTION HERE. All steps are required unless noted as optional with a blue asterisk (*).<br/><br/>INSERT DESCRIPTION HERE. <br/><br/>INSERT DESCRIPTION HERE";
    }

    [Export(typeof(IDnsModelPlugin)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    class Sample : IDnsIFrameModelPlugin
    {
        private const string EXPORT_BASENAME = "SampleDistributionExport";
        private static readonly IEnumerable<IDnsModel> _models = new[] { 
            Dns.Model( new Guid( "455C772A-DF9B-4C6B-A6B0-D4FD4DD98488" ), 
                       new Guid( QueryComposerRequestType.ModelProcessorId ),
                       "Query Composer", QueryComposerRequestType.RequestTypes.Select( t => Dns.RequestType( t.ID, t.Name, t.Description, t.ShortDescription ) ) ) };


        #region IDnsModelPlugin Members

        public string Version
        {
            get
            {
                return System.Diagnostics.FileVersionInfo.GetVersionInfo(this.GetType().Assembly.Location).FileVersion;
            }
        }

        public IEnumerable<IDnsModel> Models
        {
            get { return _models; }
        }

        public Func<HtmlHelper, IHtmlString> DisplayRequest(IDnsRequestContext context)
        {
            return null;
        }

        public Func<HtmlHelper, IHtmlString> DisplayConfigurationForm(IDnsModel model, Dictionary<string, string> properties)
        {
            ConfigModel configModel = new ConfigModel { Model = model, Properties = properties };
            return html => html.Partial<Views.Config>().WithModel(configModel);
        }
        public IEnumerable<string> ValidateConfig(ArrayList config)
        {
            var pw = (from c in config.ToArray(typeof(ConfigPostProperty)) as ConfigPostProperty[]
                      where c.Name == "Password"
                      select c).FirstOrDefault();
            var cpw = (from c in config.ToArray(typeof(ConfigPostProperty)) as ConfigPostProperty[]
                       where c.Name == "ConfirmPassword"
                       select c).FirstOrDefault();
            if (pw.Value != cpw.Value)
                return new[] { "Query Composer: Password do not match." };
            return null;
        }

        public Func<HtmlHelper, IHtmlString> DisplayResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode)
        {
            return null;
        }

        public IEnumerable<IDnsResponseExportFormat> GetExportFormats(IDnsResponseContext context)
        {
            return new[] {
                Dns.ExportFormat( "xls", "Excel" ),
                Dns.ExportFormat( "csv", "CSV" )
            };
        }

        public IDnsDocument ExportResponse( IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format, string args )
        {
            return null;
        }

        public Func<HtmlHelper, IHtmlString> EditRequestView(IDnsRequestContext context)
        {
            return null;
        }

        public Func<HtmlHelper, IHtmlString> EditRequestReDisplay( IDnsRequestContext request, IDnsPostContext post )
        {
            return null;
        }
        public DnsRequestTransaction EditRequestPost(IDnsRequestContext request, IDnsPostContext post)
        {
            return new DnsRequestTransaction
            {
                NewDocuments = null,
                UpdateDocuments = null,
                RemoveDocuments = null
            };
        }

        public void CacheMetadataResponse(Guid requestID, IDnsDataMartResponse response)
        {
        }

        public DnsResult ValidateForSubmission(IDnsRequestContext context)
        {
            return DnsResult.Success;
        }

        private bool Validate(object m, out IList<string> errorMessages)
        {
            errorMessages = null;
            return true;
        }

        public IEnumerable<IDnsResponseAggregationMode> GetAggregationModes( IDnsRequestContext context )
        {
            return null;
        }

        public DnsRequestTransaction TimeShift( IDnsRequestContext ctx, TimeSpan timeDifference )
        {
            return new DnsRequestTransaction();
        }

        public DnsResponseTransaction ExecuteRequest(IDnsRequestContext request)
        {
            return new DnsResponseTransaction();
        }

        public string EditUrl
        {
            get
            {
                return "plugins/querybuilder/edit";
            }
        }

        public string ViewUrl
        {
            get
            {
                return "plugins/querybuilder/view";
            }
        }
        #endregion

   }
}