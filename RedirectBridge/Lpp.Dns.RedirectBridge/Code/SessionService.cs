using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc;
using System.Linq.Expressions;
using System.Collections.Generic;
using Lpp.Composition;
using Lpp.Dns.Portal;

namespace Lpp.Dns.RedirectBridge
{
    [Export, PartMetadata(ExportScope.Key, TransactionScope.Id)]
    public class SessionService
    {
        public static readonly TimeSpan SessionSlidingExpirationTime = TimeSpan.FromHours(1);

        [Import]
        public IAuthenticationService Auth { get; set; }

        public PluginSession CreateSession(Guid requestID, string returnUrl, string responseToken)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();
            //var ss = Sessions.Add(new PluginSession
            //{
            //    ID = Guid.NewGuid().ToString(),
            //    ReturnUrl = returnUrl,
            //    RequestID = requestID,
            //    Expires = DateTime.Now + SessionSlidingExpirationTime,
            //    ResponseToken = responseToken,
            //    UserID = Auth.CurrentUser.ID
            //});

            //UnitOfWork.Commit();
            //return ss;
        }

        public PluginSession FindSession(int requestId)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return Sessions.All.FirstOrDefault(s => s.RequestId == requestId);
        }

        public PluginSession GetSessionNoThrow(string sessionToken, bool autoCommit)
        {
            return GetSession(sessionToken, autoCommit, () => null);
        }

        public PluginSession GetSession(string sessionToken, bool autoCommit)
        {
            return GetSession(sessionToken, autoCommit, () => { throw new FaultException<InvalidSessionFault>(InvalidSessionFault.Instance); });
        }

        PluginSession GetSession(string sessionToken, bool autoCommit, Func<PluginSession> whenBad)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var sess = sessionToken == null ? null : Sessions.All.FirstOrDefault(r => r.Id == sessionToken);
            //if (sess == null) return whenBad();
            //if (sess.IsCommitted || sess.IsAborted) return whenBad();

            //if (sess.Expires <= DateTime.Now)
            //{
            //    Sessions.Remove(sess);
            //    UnitOfWork.Commit();
            //    return whenBad();
            //}

            //var user = Users.Find(sess.UserId);
            //if (user == null) return whenBad();

            //sess.Expires = DateTime.Now + SessionSlidingExpirationTime;
            //Auth.SetCurrentUser(user, AuthenticationScope.Transaction);
            //if (autoCommit) UnitOfWork.Commit();

            //return sess;
        }
    }
}