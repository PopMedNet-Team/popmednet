using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Collections.Generic;
using Lpp.Composition;
using Lpp.Dns.Portal;
using Lpp.Dns.RedirectBridge;
using Lpp.Dns.Data;

// TODO Ripped from Redirect. Refactor.
namespace Lpp.Dns.RemotePlugin
{
    [Export, PartMetadata(ExportScope.Key, TransactionScope.Id)]
    public class SessionService
    {
        public static readonly TimeSpan SessionSlidingExpirationTime = TimeSpan.FromHours(1);
        //[Import]
        //public IRepository<RedirectDomain, PluginSession> Sessions { get; set; }
        //[Import]
        //public IRepository<Lpp.Dns.Model.DnsDomain, Lpp.Dns.Model.User> Users { get; set; }
        //[Import]
        //public IUnitOfWork<RedirectDomain> UnitOfWork { get; set; }
        [Import]
        public IAuthenticationService Auth { get; set; }

        public PluginSession CreateSession(Project project)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var ss = Sessions.Add(new PluginSession
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Expires = DateTime.Now + SessionSlidingExpirationTime,

            //    ProjectId = project.SID,
            //    UserId = Auth.CurrentUser.Id
            //});

            //UnitOfWork.Commit();
            //return ss;
        }

        public PluginSession GetSession(string sessionToken, bool autoCommit)
        {
            //Contract.Ensures(//Contract.Result<PluginSession>() != null);
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