using System;
using System.Linq;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using System.IO;
using Lpp.Composition;
using Lpp.Mvc;
using Lpp.Dns.Portal;
using System.Collections;
using System.Data.Entity.SqlServer;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;

namespace Lpp.Dns.RedirectBridge
{
    [Export(typeof(IDnsModelPlugin)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    public class RedirectModelPlugin : IDnsModelPlugin
    {
        //[Import]
        //public IRepository<RedirectDomain, Model> Models { get; set; }
        //[Import]
        //public IRepository<RedirectDomain, RequestType> RequestTypes { get; set; }
        [Import]
        public SessionService Sessions { get; set; }
        //[Import]
        //public IRepository<RedirectDomain, PluginSessionDocument> Documents { get; set; }
        //[Import]
        //public IUnitOfWork<RedirectDomain> UnitOfWork { get; set; }
        [Import]
        public IDocumentService DocService { get; set; }

        public DataContext DataContext
        {
            get
            {
                return System.Web.HttpContext.Current.Items["DataContext"] as DataContext;
            }
        }

        public string Version
        {
            get
            {
                return System.Diagnostics.FileVersionInfo.GetVersionInfo(this.GetType().Assembly.Location).FileVersion;
            }
        }

        IEnumerable<IDnsModel> IDnsModelPlugin.Models
        {
            get
            {
                //TODO: this should pull from the plugin models table.
                return new IDnsModel[0];
                //return Models.All
                //    .AsEnumerable()
                //    .Select(m => Dns.Model(m.Id, m.ModelProcessorId, m.Name,
                //      m.RequestTypes
                //      .Select(r => Dns.RequestType(r.LocalId, r.Name, r.Description, /*r.ShortDescription*/ "", r.IsMetadataRequest))));
            }
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
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var docs = from d in context.DataMartResponses
            //           from doc in d.Documents
            //           select doc;

            //var dm = (
            //    from m in Models.All
            //    from r in m.RequestTypes
            //    where r.LocalId == context.Request.RequestType.Id
            //    select new { m, r }
            //    ).FirstOrDefault();

            //return html => html.Partial<Views.Bridge.ResponseView>().WithModel(new Models.ResponseDetailModel
            //{
            //    Context = context,
            //    Model = dm.m,
            //    RequestType = dm.r
            //});
        }

        public Func<HtmlHelper, IHtmlString> EditRequestView(IDnsRequestContext context)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var sess = Sessions.FindSession(context.RequestId);
            //if (sess != null)
            //{
            //    if (sess.IsCommitted) return DisplayUnsubmittedRequest(sess);
            //    if (sess.IsAborted) return html => html.Partial<Views.Bridge.RequestIsAborted>().WithoutModel();
            //}

            //var rt = RequestTypes.All.FirstOrDefault(r => r.LocalId == context.RequestType.Id && r.Model.Id == context.Model.Id);
            //if (rt == null) return html => html.Partial<Views.Bridge.UnknownRequestType>().WithoutModel();

            //return html => html.Partial<Views.Bridge.RequestRedirectPrompt>().WithModel(new Models.RedirectPromptModel
            //{
            //    Context = context,
            //    RequestType = rt
            //});
        }

        public Func<HtmlHelper, IHtmlString> EditRequestReDisplay(IDnsRequestContext request, IDnsPostContext post)
        {
            return EditRequestView(request);
        }

        private Func<HtmlHelper, IHtmlString> DisplayUnsubmittedRequest(PluginSession session)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();
            //return DocService.GetVisualization(DocumentsFrom(session));
        }

        public DnsRequestTransaction EditRequestPost(IDnsRequestContext request, IDnsPostContext post)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var session = Sessions.FindSession(request.RequestId);
            //if (session == null) return DnsRequestTransaction.Failed("Unknown Request ID");
            //if (session.IsAborted) return DnsRequestTransaction.Failed("Request has been aborted");
            //if (!session.IsCommitted) return DnsRequestTransaction.Failed("Request creation is not finished yet");

            //return new DnsRequestTransaction
            //{
            //    RemoveDocuments = request.Documents,
            //    NewDocuments = DocumentsFrom(session)
            //};
        }

        private IEnumerable<DbDocument> DocumentsFrom(PluginSession session)
        {
            return DocumentsFrom(session.Documents.AsQueryable());
        }

        public static IEnumerable<DbDocument> DocumentsFrom(IQueryable<PluginSessionDocument> docs)
        {
            return docs
                .Select(d => new { d.ID, d.Name, d.MimeType, d.IsViewable, Size = SqlFunctions.DataLength(d.Body) })
                .AsEnumerable()
                .Select(d => new DbDocument(docs, d.ID) { Name = d.Name, BodySize = d.Size ?? 0, Viewable = d.IsViewable, MimeType = d.MimeType });
        }

        private Func<IDnsDataMart, bool> GetDataMartsFilter(byte[] serializedIds)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //if (serializedIds == null || serializedIds.Length % 4 != 0) return _ => true;

            //var ids = Enumerable
            //    .Range(0, serializedIds.Length / 4)
            //    .Select(i => BitConverter.ToInt32(serializedIds, i * 4))
            //    .ToDictionary(id => id);

            //return m => ids.ContainsKey(m.Id);
        }

        public class DbDocument : IDnsPersistentDocument
        {
            private readonly IQueryable<PluginSessionDocument> _documents;
            public Guid ID { get; private set; }

            public DbDocument(IQueryable<PluginSessionDocument> docs, Guid id)
            {
                //Contract.Requires( docs != null );
                ID = id;
                _documents = docs;
            }

            public Stream ReadStream()
            {
                return new MemoryStream(_documents.Where(dd => dd.ID == ID).Select(dd => dd.Body).FirstOrDefault() ?? new byte[0]);
            }

            public string ReadStreamAsString()
            {
                using (var stream = new StreamReader(ReadStream()))
                {
                    return stream.ReadToEnd();
                }
            }

            public long BodySize { get; set; }
            public string Name { get; set; }
            public string MimeType { get; set; }
            public bool Viewable { get; set; }
            public string Kind { get; set; }

            public string FileName { get; set; }
        }

        public void CacheMetadataResponse(Guid requestID, IDnsDataMartResponse response)
        {
            // Do nothing
        }

        public IEnumerable<IDnsResponseExportFormat> GetExportFormats(IDnsResponseContext context)
        {
            return null;
        }

        public IDnsDocument ExportResponse(IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format, string args)
        {
            return null;
        }

        public DnsResult ValidateForSubmission(IDnsRequestContext context)
        {
            return DnsResult.Success;
        }

        public IEnumerable<IDnsResponseAggregationMode> GetAggregationModes(IDnsRequestContext context)
        {
            return null;
        }

        public DnsRequestTransaction TimeShift(IDnsRequestContext ctx, TimeSpan timeDifference)
        {
            return new DnsRequestTransaction();
        }

        public DnsResponseTransaction ExecuteRequest(IDnsRequestContext request)
        {
            throw new NotImplementedException();
        }
    }
}