using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Linq;
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
using Lpp.Data;
using Lpp.Dns.DocumentVisualizers.Views;
using Lpp.Mvc;
using Lpp.Dns.Portal;
using Lpp.Security;
using Lpp.Dns.Model;
using Lpp.Dns.PubHealth.Models;
using Lpp.Dns.PubHealth.Views;
using Lpp.Dns;
//using Lpp;



namespace Lpp.Dns.PubHealth
{
    [Export(typeof(IDnsModelPlugin)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    class PubHealthPlugin : IDnsModelPlugin
    {

        private static readonly IEnumerable<IDnsModel> _models = new[] { 
            Lpp.Dns.Dns.Model( new Guid( "f77cf120-1431-4481-8d88-da44ba61239b" ), 
                       new Guid( "1912caca-8b19-4657-9cbb-39804cd2fad4" ),
                       "Public Health", TestRequestType.RequestTypes.Select( t => Lpp.Dns.Dns.RequestType( t.Id, t.Name, t.Description, t.ShortDescription ) ) ) };

        private PubHealthModel GetModel(IDnsRequestContext context)
        {
            return new PubHealthModel();
        }

        public static PubHealthModel InitializeModel(PubHealthModel m, IDnsRequestContext request)
        {
            m.RequestType = TestRequestType.All.FirstOrDefault(rt => rt.Id == request.RequestType.Id);
            m.RequestId = request.RequestId;
            //m.MinDate = "";
            //m.MaxDate = "";

            if (request.Documents != null && request.Documents.Count() > 0)
            {
                IDnsPersistentDocument doc = request.Documents.FirstOrDefault(s => s.Kind == Document.DocumentKind_Request);
                if (doc != null)
                {
                    string docContents = new StreamReader(doc.OpenBody()).ReadToEnd();
                    string[] parts = docContents.Split('|');
                    //m.MinDate = parts[0];
                    //m.MaxDate = parts[1];

                }
            }
            return m;
        }

        #region IDnsModelPlugin Members

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
            return null;
        }

        public IEnumerable<string> ValidateConfig(ArrayList config)
        {
            return null;
        }

        public Func<HtmlHelper, IHtmlString> DisplayResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode)
        {
            return null;
        }

        public IEnumerable<IDnsResponseExportFormat> GetExportFormats(IDnsResponseContext context)
        {
            return null;
        }

        public IDnsDocument ExportResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format)
        {
            throw new NotImplementedException();
        }

        public Func<HtmlHelper, IHtmlString> EditRequestView(IDnsRequestContext context)
        {
            return html => html.Partial<Create>().WithModel(InitializeModel(GetModel(context), context));
        }

        public Func<HtmlHelper, IHtmlString> EditRequestReDisplay(IDnsRequestContext request, IDnsPostContext post)
        {
            return html => html.Partial<Create>().WithModel(InitializeModel(post.GetModel<PubHealthModel>(), request));
        }
        public DnsRequestTransaction EditRequestPost(IDnsRequestContext request, IDnsPostContext post)
        {
            System.Collections.Generic.List<Lpp.Dns.IDnsDocument> newDocuments = new System.Collections.Generic.List<Lpp.Dns.IDnsDocument>();
            System.Collections.Generic.List<Lpp.Dns.IDnsPersistentDocument> removeDocuments = new System.Collections.Generic.List<Lpp.Dns.IDnsPersistentDocument>();
            var m = post.GetModel<PubHealthModel>();

            string docContents = ""; // m.MinDate + "|" + m.MaxDate;

            newDocuments.Add(Dns.Document(request.Header.Name, "text/plain", true, Document.DocumentKind_Request, () => new MemoryStream(GetBytes(docContents)), () => docContents.Length));
            IDnsPersistentDocument doc = request.Documents.FirstOrDefault(s => s.Kind == Document.DocumentKind_Request);
            if (doc != null)
                removeDocuments.Add(doc);

            return new DnsRequestTransaction
            {
                NewDocuments = newDocuments,
                UpdateDocuments = null,
                RemoveDocuments = removeDocuments
            };
        }

        static byte[] GetBytes(string str)
        {
            return new System.Text.ASCIIEncoding().GetBytes(str);
        }

        public void CacheMetadataResponse(IDnsDataMartResponse response)
        {
        }

        public DnsResult ValidateForSubmission(IDnsRequestContext context)
        {
            PubHealthModel m = GetModel(context);
            IList<string> errorMessages;
            if (Validate(m, out errorMessages))
                return DnsResult.Success;
            else
                return DnsResult.Failed(errorMessages.ToArray<string>());
        }

        private bool Validate(PubHealthModel m, out IList<string> errorMessages)
        {
            errorMessages = new List<string>();
            return errorMessages.Count > 0 ? false : true;
        }

        public IEnumerable<IDnsResponseAggregationMode> GetAggregationModes(IDnsRequestContext context)
        {
            return null;
        }

        public DnsRequestTransaction TimeShift(IDnsRequestContext ctx, TimeSpan timeDifference)
        {
            return new DnsRequestTransaction();
        }

        public DnsResponseTransaction ExecuteRequest(IDnsRequestContext context)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}