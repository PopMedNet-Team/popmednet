using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Mvc;
using log4net;
using Lpp.Dns.Portal;
using Lpp.Dns.RedirectBridge;
using Lpp.Dns.RedirectBridge.Controllers;
using Lpp.Mvc;
using Lpp.Utilities.Legacy;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.RemotePlugin
{
    public class RemotePluginService : IRemotePluginService
    {
        [Import]
        public ILog Log { get; set; }
        [Import]
        public SessionService Sessions { get; set; }
        //[Import]
        //public IRepository<DnsDomain, Model.Request> Requests { get; set; }
        [Import]
        public IRequestService RequestService { get; set; }
        [Import]
        public IAuthenticationService Auth { get; set; }
        //[Import]
        //public IRepository<DnsDomain, User> Users { get; set; }
        //[Import]
        //public IRepository<DnsDomain, Project> Projects { get; set; }
        //[Import]
        //public IRepository<RedirectDomain, PluginSessionDocument> PluginDocuments { get; set; }
        //[Import]
        //public IRepository<DnsDomain, Model.Document> Documents { get; set; }
        //[Import]
        //public IUnitOfWork<RedirectDomain> UnitOfWork { get; set; }
        //[Import]
        //public IUnitOfWork<DnsDomain> DnsUnitOfWork { get; set; }
        [Import]
        public IPluginService Plugins { get; set; }
        [Import]
        public HttpContextBase HttpContext { get; set; }

        public string StartSession(Credentials credentials, string requestor, Guid projectId)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //try
            //{
            //    if (Authorize(credentials))
            //    {
            //        try
            //        {
            //            Auth.SetCurrentUser((from u in Users.All
            //                                 where u.Username == requestor
            //                                 select u).FirstOrDefault(), AuthenticationScope.Transaction);
            //        }
            //        catch
            //        {
            //            string message = "Requestor: " + requestor + " is not a valid user on the system.";
            //            Log.Debug(message);
            //            OutgoingWebResponseContext ctx = WebOperationContext.Current.OutgoingResponse;
            //            ctx.StatusCode = System.Net.HttpStatusCode.BadRequest;
            //            ctx.StatusDescription = message;
            //            return null;
            //        }

            //        try
            //        {
            //            var session = Sessions.CreateSession(Projects.Find(projectId));
            //            return Convert.ToBase64String(Encoding.ASCII.GetBytes(session.Id));
            //        }
            //        catch
            //        {
            //            string message = "Project ID: " + projectId + " is not found on the system.";
            //            Log.Debug(message);
            //            OutgoingWebResponseContext ctx = WebOperationContext.Current.OutgoingResponse;
            //            ctx.StatusCode = System.Net.HttpStatusCode.BadRequest;
            //            ctx.StatusDescription = message;
            //            return null;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.Debug(ex.Message);
            //    OutgoingWebResponseContext ctx = WebOperationContext.Current.OutgoingResponse;
            //    ctx.StatusCode = System.Net.HttpStatusCode.BadRequest;
            //    ctx.StatusDescription = ex.Message;
            //}

            //return null;
        }

        public string StartProjectSession(Credentials credentials, string requestor, string projectAcronym)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //try
            //{
            //    if (Authorize(credentials))
            //    {
            //        try
            //        {
            //            Auth.SetCurrentUser((from u in Users.All
            //                                 where u.Username == requestor
            //                                 select u).FirstOrDefault(), AuthenticationScope.Transaction);
            //        }
            //        catch
            //        {
            //            string message = "Requestor: " + requestor + " is not a valid user on the system.";
            //            Log.Debug(message);
            //            OutgoingWebResponseContext ctx = WebOperationContext.Current.OutgoingResponse;
            //            ctx.StatusCode = System.Net.HttpStatusCode.BadRequest;
            //            ctx.StatusDescription = message;
            //            return null;
            //        }

            //        try
            //        {
            //            var session = Sessions.CreateSession((from p in Projects.All
            //                                                  where p.Acronym == projectAcronym
            //                                                  select p).FirstOrDefault());
            //            return Convert.ToBase64String(Encoding.ASCII.GetBytes(session.Id));
            //        }
            //        catch
            //        {
            //            string message = "Project: " + projectAcronym + " is not found on the system.";
            //            Log.Debug(message);
            //            OutgoingWebResponseContext ctx = WebOperationContext.Current.OutgoingResponse;
            //            ctx.StatusCode = System.Net.HttpStatusCode.BadRequest;
            //            ctx.StatusDescription = message;
            //            return null;
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.Debug(ex.Message);
            //    OutgoingWebResponseContext ctx = WebOperationContext.Current.OutgoingResponse;
            //    ctx.StatusCode = System.Net.HttpStatusCode.BadRequest;
            //    ctx.StatusDescription = ex.Message;
            //}

            //return null;
        }

        public void CloseSession(string sessionToken)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //string sessionId = Encoding.ASCII.GetString(Convert.FromBase64String(sessionToken));
            //Sessions.GetSession(sessionId, false).IsCommitted = true;
            //UnitOfWork.Commit();
        }

        public string CreateRequest(string sessionToken, string requestTypeId)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //try
            //{
            //    string sessionId = Encoding.ASCII.GetString(Convert.FromBase64String(sessionToken));
            //    var session = Sessions.GetSession(sessionId, false);

            //    var rt = Plugins.GetPluginRequestType(Guid.Parse(requestTypeId));
            //    if (rt == null)
            //    {
            //        string msg = "Plugin for request type: " + requestTypeId + " is not installed.";
            //        Log.Debug(msg);
            //        OutgoingWebResponseContext ctx = WebOperationContext.Current.OutgoingResponse;
            //        ctx.StatusCode = System.Net.HttpStatusCode.BadRequest;
            //        ctx.StatusDescription = msg;
            //        return null;
            //    }

            //    var req = RequestService.CreateRequest(Projects.Find(session.ProjectId), rt);

            //    session.RequestId = req.RequestId;

            //    // TODO[ddee] Do we want to allow each session to create multiple requests?

            //    DnsUnitOfWork.Commit();
            //    return req.RequestId.ToString();
            //}
            //catch (Exception ex)
            //{
            //    Log.Debug(ex.Message);
            //    OutgoingWebResponseContext ctx = WebOperationContext.Current.OutgoingResponse;
            //    ctx.StatusCode = System.Net.HttpStatusCode.BadRequest;
            //    ctx.StatusDescription = ex.Message;
            //}

            //return null;
        }

        //public IEnumerable<RemotePlugin.DataMart> GetApplicableDataMarts(string sessionToken, string requestId)
        //{
        //    string sessionId = Encoding.ASCII.GetString(Convert.FromBase64String(sessionToken));
        //    var session = Sessions.GetSession(sessionId, false);

        //    if (session != null && !session.IsAborted && !session.IsCommitted)
        //    {
        //        IList<Model.DataMart> modelDatamarts = RequestService.GetRequestContext(Convert.ToInt32(requestId)).DataMarts as IList<Model.DataMart>;
        //        var datamarts = from x in modelDatamarts
        //                        select new RemotePlugin.DataMart()
        //                               {
        //                                   Id = x.Id,
        //                                   Name = x.Name
        //                               };
        //        return datamarts;
        //    }

        //    return null;
        //}

        public void PostDocument(string sessionToken, string requestId, string documentName, string documentMimeType, bool isViewable, byte[] documentBody)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //string sessionId = Encoding.ASCII.GetString(Convert.FromBase64String(sessionToken));
            //var session = Sessions.GetSession(sessionId, false);

            //Documents.Add(new Model.Document
            //{
            //    MimeType = documentMimeType,
            //    Name = documentName,
            //    IsViewable = isViewable,
            //    Data = documentBody,
            //    FileName = documentName
            //});

            //UnitOfWork.Commit();
        }


        public void SubmitRequest(string sessionToken, string requestId, RequestHeader requestHeader, int[] applicableDatamarts)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //Log.Debug("Start submit request");
            //string sessionId = Encoding.ASCII.GetString(Convert.FromBase64String(sessionToken));
            //var session = Sessions.GetSession(sessionId, false);
            //var ctx = RequestService.GetRequestContext(Convert.ToInt32(requestId));

            //if (requestHeader != null)
            //{
            //    Log.Debug("Processing request header");
            //    var h = ctx.Header;
            //    h.Name = requestHeader.Name;
            //    h.DueDate = requestHeader.DueDate;
            //    h.Priority = MapPriority(requestHeader.Priority) ?? h.Priority;

            //    ctx.ModifyMetadata(new DnsRequestMetadata
            //    {
            //        Header = h
            //    });
            //}

            //Log.Debug("Get documents");
            //var docs = from d in PluginDocuments.All
            //           where d.Session.Id == sessionId
            //           select d;

            //foreach (PluginSessionDocument doc in docs)
            //{
            //    Documents.Add(new Model.Document
            //                                {
            //                                    Name = doc.Name,
            //                                    IsViewable = doc.IsViewable,
            //                                    MimeType = doc.MimeType,
            //                                    FileName = doc.FileName,
            //                                    Data = doc.Body
            //                                });
            //}

            //Log.Debug("Submitting request");
            //var res = RequestService.SubmitRequest(ctx);
            //if (!res.IsSuccess)
            //{
            //    Log.Debug("Failed to submit request");
            //    foreach (var s in res.ErrorMessages)
            //    {
            //        Log.Debug(s);
            //    }
            //    throw new Exception("Failed to submit request.");
            //}

            //Log.Debug("Request successfully submitted");
            //session.IsCommitted = true;
            //DnsUnitOfWork.Commit();
        }

        public List<RemotePlugin.Request> GetRequests(string sessionToken, DateTime? fromDate, DateTime? toDate, List<int> filterByDataMartIds, List<RemotePlugin.RequestStatus> filterByStatus)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //string sessionId = Encoding.ASCII.GetString(Convert.FromBase64String(sessionToken));
            //Log.Debug("GetRequests: Session id is " + sessionId + " for session token: " + sessionToken);
            //var session = Sessions.GetSession(sessionId, false);
            //var url = new UrlHelper(HttpContext.Request.RequestContext);
            //var grantedRequests = RequestService.GetGrantedRequests(Projects.Find(session.ProjectId));

            //// FIXME The following needs rework as it is inefficient to pull
            //// everything into memory before filtering.
            //List<RemotePlugin.Request> rList = new List<Request>();
            //foreach (Model.Request r in grantedRequests)
            //{
            //    // Filtered by datamarts.
            //    if (filterByDataMartIds != null)
            //    {
            //        var dm = from dr in r.Routings
            //                 where filterByDataMartIds.Contains(dr.DataMartId)
            //                 select dr;

            //        if (dm == null)
            //            continue;

            //    }

            //    // Filtered by status.
            //    if (filterByStatus != null && filterByStatus.Any())
            //    {
            //        var ss = filterByStatus.Select(s => (int)s);
            //        var st = from dm in r.Routings
            //                 where ss.Contains((int)dm.RequestStatus)
            //                 select dm;

            //        if (st == null)
            //            continue;
            //    }

            //    if (r.Submitted != null && (fromDate == null || r.Submitted >= fromDate) && (toDate == null || r.Submitted <= toDate))
            //    {
            //        var rr = new Lpp.Dns.RemotePlugin.Request
            //        {
            //            Id = r.Id,
            //            Description = r.Description,
            //            Name = r.Name,
            //            CreateDate = r.Created,
            //            RequestTypeId = r.RequestTypeId.ToString(),
            //            DataMartResponses = (from dm in r.Routings.EmptyIfNull()
            //                                 select new RemotePlugin.DataMartResponse
            //                                 {
            //                                     DataMart = new RemotePlugin.DataMart { Id = dm.DataMartId, Name = dm.DataMart.Name },
            //                                     Documents = (from i in dm.Instances
            //                                                  where i.IsCurrent
            //                                                  from d in i.Documents
            //                                                  select new RemotePlugin.Document
            //                                                  {
            //                                                      ID = d.ID,
            //                                                      Name = d.Name,
            //                                                      MimeType = d.MimeType,
            //                                                      Size = (int)d.Length,
            //                                                      LiveUrl = url.Absolute(url.Action<DocumentController>(c => c.Document(sessionToken, d.ID)))
            //                                                  })
            //                                                  .ToArray()
            //                                 })
            //                                 .ToList()
            //        };

            //        rList.Add(rr);
            //    }
            //}

            //return rList;

        }

        public Request GetRequest(string sessionToken, string requestId)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //try
            //{
            //    string sessionId = Encoding.ASCII.GetString(Convert.FromBase64String(sessionToken));
            //    Log.Debug("GetRequest: Session id is " + sessionId + " for session token: " + sessionToken);
            //    var session = Sessions.GetSession(sessionId, false);
            //    session.RequestId = Convert.ToInt32(requestId);
            //}
            //catch (Exception ex)
            //{
            //    string msg = "GetRequest is unable to get session for session token: " + sessionToken;
            //    Log.Debug(msg, ex);
            //    OutgoingWebResponseContext ctx = WebOperationContext.Current.OutgoingResponse;
            //    ctx.StatusCode = System.Net.HttpStatusCode.BadRequest;
            //    ctx.StatusDescription = msg + " - " + ex.Message;
            //}

            //try
            //{
            //    var url = new UrlHelper(HttpContext.Request.RequestContext);
            //    var r = RequestService.GetRequestContext(Convert.ToInt32(requestId)).Request;
            //    Log.Debug("GetRequest: RequestService return RequestContext for request id is " + requestId);

            //    try
            //    {
            //        Lpp.Dns.RemotePlugin.Request rr = new Lpp.Dns.RemotePlugin.Request
            //        {
            //            Id = r.Id,
            //            Description = r.Description,
            //            Name = r.Name,
            //            CreateDate = r.Created,
            //            Documents = (from d in Documents.All
            //                         where d.ItemID == r.SID
            //                         select new RemotePlugin.Document
            //                         {
            //                             ID = d.ID,
            //                             Name = d.Name,
            //                             MimeType = d.MimeType,
            //                             Size = (int)d.Length,
            //                             Content = d.Data
            //                         }).ToList(),
            //            DataMartResponses = new List<DataMartResponse>()
            //        };

            //        Log.Debug("GetRequest: Remote plugin request created and documents retrieved for request id is " + requestId);

            //        try
            //        {
            //            foreach (var rt in r.Routings)
            //            {
            //                var dm = rt.DataMart;
            //                var dmr = new RemotePlugin.DataMartResponse
            //                {
            //                    DataMart = new RemotePlugin.DataMart
            //                    {
            //                        Id = dm.Id,
            //                        Name = dm.Name
            //                    }
            //                };

            //                IList<RemotePlugin.Document> docs = new List<RemotePlugin.Document>();
            //                foreach (var d in rt.Instances.Where(i => i.IsCurrent).SelectMany(i => i.Documents))
            //                {
            //                    docs.Add(new RemotePlugin.Document
            //                    {
            //                        ID = d.ID,
            //                        Name = d.Name,
            //                        MimeType = d.MimeType,
            //                        Size = (int)d.Length,

            //                        Content = d.Data,

            //                        LiveUrl = url.Absolute(url.Action<DocumentController>(c => c.Document(sessionToken, d.ID)))
            //                    });
            //                }
            //                dmr.Documents = docs.ToArray();

            //                rr.DataMartResponses.Add(dmr);
            //            }

            //            Log.Debug("GetRequest: DataMartResponses setup for request id is " + requestId);

            //            return rr;
            //        }
            //        catch (Exception ex)
            //        {
            //            string msg = "Unable to setup DataMartResponses for request " + requestId;
            //            Log.Debug(msg, ex);
            //            OutgoingWebResponseContext ctx = WebOperationContext.Current.OutgoingResponse;
            //            ctx.StatusCode = System.Net.HttpStatusCode.BadRequest;
            //            ctx.StatusDescription = msg + " - " + ex.Message;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        string msg = "Unable to create remote request for id " + requestId;
            //        Log.Debug(msg, ex);
            //        OutgoingWebResponseContext ctx = WebOperationContext.Current.OutgoingResponse;
            //        ctx.StatusCode = System.Net.HttpStatusCode.BadRequest;
            //        ctx.StatusDescription = msg + " - " + ex.Message;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    string msg = "Unable to get Request for request id: " + requestId;
            //    Log.Debug(msg, ex);
            //    OutgoingWebResponseContext ctx = WebOperationContext.Current.OutgoingResponse;
            //    ctx.StatusCode = System.Net.HttpStatusCode.BadRequest;
            //    ctx.StatusDescription = msg + " - " + ex.Message;
            //}

            //return null;
        }

        private bool Authorize(Credentials login)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //try
            //{
            //    if (login == null || login.Password == null || string.IsNullOrEmpty(login.Username)) throw new AuthenticationFault();

            //    var pwdHash = Password.ComputeHash(login.Password);
            //    var user = login == null ? null : Users.All.FirstOrDefault(u => u.Username == login.Username && u.Password == pwdHash);
            //    if (user == null) throw new AuthenticationFault();

            //    return true;
            //}
            //catch (FaultException ex)
            //{
            //    Log.Error(ex);
            //    throw;
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex);
            //    InternalErrorFault fault = new InternalErrorFault();
            //    fault.ErrorMessages.ToList().Add(ex.Message);
            //    if (ex.InnerException != null)
            //        fault.ErrorMessages.ToList().Add(ex.InnerException.Message);
            //    throw fault;
            //}

        }
    }

    // TODO Ripped from Portal/Code/Password. Refactor.
    static class Password
    {
        public static string ComputeHash(string password)
        {
            //Contract.Requires(password != null);
            //Contract.Ensures(//Contract.Result<string>() != null);

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                sha.Initialize();
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }
    }

}

