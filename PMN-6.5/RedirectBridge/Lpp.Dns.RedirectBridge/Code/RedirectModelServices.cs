using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Mvc;
using log4net;
using Lpp.Dns.Portal;
using Lpp.Mvc;

namespace Lpp.Dns.RedirectBridge
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public sealed class RedirectModelServices : IRequestService, IResponseService
    {
        [Import]
        public HttpContextBase HttpContext { get; set; }
        [Import]
        public IDnsModelPluginHost Host { get; set; }
        [Import]
        public SessionService Sessions { get; set; }
        //[Import]
        //public IRepository<RedirectDomain, PluginSessionDocument> Documents { get; set; }
        //[Import]
        //public IUnitOfWork<RedirectDomain> UnitOfWork { get; set; }
        [Import]
        public IAuthenticationService Auth { get; set; }
        //[Import]
        //public IRepository<DnsDomain, User> Users { get; set; }
        [Import]
        public ILog Log { get; set; }

        public void RequestCreated(string sessionToken, RequestHeader requestHeader, Guid[] applicableDataMartIDs)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //Call(sessionToken, sess =>
            //{
            //    var ctx = Host.GetRequestContext(sess.RequestId);
            //    if (ctx == null) return;

            //    if (requestHeader != null || (applicableDataMartIds != null && applicableDataMartIds.Any()))
            //    {
            //        var h = ctx.Header;
            //        if (requestHeader != null)
            //        {
            //            h.Name = requestHeader.Name ?? h.Name;
            //            h.Description = requestHeader.Description ?? h.Description;
            //            h.Activity = requestHeader.Activity == null ? h.Activity :
            //                ctx.Activities.FirstOrDefault(a => string.Equals(a.Name, requestHeader.Activity, StringComparison.InvariantCultureIgnoreCase));
            //            h.ActivityDescription = requestHeader.ActivityDescription ?? h.ActivityDescription;
            //            h.DueDate = requestHeader.DueDate ?? h.DueDate;
            //            h.Priority = MapPriority(requestHeader.Priority) ?? h.Priority;
            //        }

            //        ctx.ModifyMetadata(new DnsRequestMetadata
            //        {
            //            Header = h,
            //            DataMartFilter = applicableDataMartIds != null ? dm => applicableDataMartIds.Contains(dm.Id) : (Func<IDnsDataMart, bool>)null
            //        });
            //    }

            //    sess.IsCommitted = true;
            //    UnitOfWork.Commit();
            //});
        }

        public void RequestAborted(string sessionToken)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //Call(sessionToken, sess =>
            //{
            //    sess.IsAborted = true;
            //    UnitOfWork.Commit();
            //});
        }

        public DataMartList GetApplicableDataMarts(string sessionToken)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return Call(sessionToken, sess =>
            //    new DataMartList(
            //        Host
            //        .GetRequestContext(sess.RequestId)
            //        .DataMarts
            //        .Select(DataMart)
            //    )
            //);
        }

        public void PostDocument(string sessionToken, string documentName, string documentMimeType, bool isViewable, byte[] documentBody)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //Call(sessionToken, sess =>
            //{
            //    Documents.Add(new PluginSessionDocument
            //    {
            //        Session = sess,
            //        MimeType = documentMimeType,
            //        Name = documentName,
            //        IsViewable = isViewable,
            //        Body = documentBody
            //    });
            //    UnitOfWork.Commit();
            //});
        }

        SessionMetadata IRequestService.GetSessionMetadata(string sessionToken)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return Call(sessionToken, sess => GetSessionMetadata(sess, Host.GetRequestContext(sess.RequestId)));
        }

        ResponseSessionMetadata IResponseService.GetSessionMetadata(string sessionToken)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return Call(sessionToken, sess =>
            //{
            //    var resp = Host.GetResponseContext(sess.RequestId, sess.ResponseToken);
            //    var url = new UrlHelper(HttpContext.Request.RequestContext);

            //    return new ResponseSessionMetadata
            //    {
            //        Session = GetSessionMetadata(sess, resp.Request),
            //        DataMartResponses =
            //            resp.DataMartResponses
            //            .Select(r => new DataMartResponse
            //            {
            //                DataMart = DataMart(r.DataMart),
            //                Documents =
            //                r.Documents
            //                    .Select(d => new Document
            //                    {
            //                        ID = d.ID,
            //                        Name = d.Name,
            //                        MimeType = d.MimeType,
            //                        Size = d.BodySize,
            //                        LiveUrl = url.Absolute(
            //                            url.Action<Controllers.DocumentController>(c => c.Document(sessionToken, d.ID)))
            //                    })
            //                    .ToArray()
            //            })
            //            .ToArray()
            //    };
            //});
        }

        private SessionMetadata GetSessionMetadata(PluginSession sess, IDnsRequestContext req)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return new SessionMetadata
            //{
            //    RequestId = sess.RequestID,
            //    ReturnUrl = sess.ReturnUrl,
            //    ModelId = req.Model.Id.ToString(),
            //    RequestTypeId = req.RequestType.Id.ToString()
            //};
        }

        private DataMart DataMart(IDnsDataMart m)
        {
            return new DataMart
            {
                ID = m.ID,
                Name = m.Name,
                Metadata = m.MetadataDocuments.Select(d => new Document
                {
                    ID = d.ID,
                    MimeType = d.MimeType,
                    Size = d.Length
                })
                .ToArray()
            };
        }

        private T Call<T>(string sessionToken, Func<PluginSession, T> action, bool autoCommitSession = false)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //try
            //{
            //    var sess = Sessions.GetSession(sessionToken, autoCommitSession);
            //    Auth.SetCurrentUser(Users.Find(sess.UserId), AuthenticationScope.Transaction);
            //    return action(sess);
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex);
            //    //System.IO.File.WriteAllText( "C:\\a.txt", ex.ToString() );
            //    throw;
            //}
        }

        private void Call(string sessionToken, Action<PluginSession> action, bool autoCommitSession = true)
        {
            Call(sessionToken, s => { action(s); return 0; }, autoCommitSession);
        }
    }
}