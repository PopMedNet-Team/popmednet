using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using log4net;
using Lpp;
using Lpp.Composition;
using Lpp.Dns.Data;
using Lpp.Utilities.Security;

namespace Lpp.Dns.Portal
{
    //Jamie removed because it's broken.

    /// <summary>
    /// This service is here for optimization purposes. There are a few very complex queries in our system,
    /// which take EF ages to compile into SQL (ages == on the order of 10 seconds). Thanks to compiled query caching,
    /// this performance hit will only happen on the very first execution of such queries, but still very
    /// unpleasant user experience.
    /// To help this situation, this service will activate on application start and run those queries once
    /// in the background, thus ensuring they are already precompiled by the time the user actually gets to them.
    /// </summary>
    [Export(typeof(IHttpModule))]
    public class QueryPrecompileService : IHttpModule
    {
        void RunQueries(ICompositionService comp)
        {
            //var rq = comp.Get<IRepository<Model.DnsDomain, Model.Request>>().All.FirstOrDefault();
            //if ( rq == null ) return;

            //Log.Info( "Precompiling: VirtualResponse - Get by ID" );
            //comp.Get<IResponseService>().GetVirtualResponses( rq.Id, "1,2,3" ).FirstOrDefault();
            //Log.Info( "Precompiling: VirtualResponse - Get by ID     (done)" );

            //var ctr = comp.Get<Controllers.RequestController>();
            //var ctx = comp.Get<IRequestService>().GetRequestContext( rq.Id );

            //var allSorts = from sk in Controllers.RequestController.ResponsesSort.AllKeys.StartWith( "" )
            //               let sd = Controllers.RequestController.ResponsesSort.GetSortDefinition( sk, null )
            //               from asc in new[] { sd.IsAscending, !sd.IsAscending }
            //               from page in new[] { "0", "1" }
            //               select new Models.RequestChildrenGetModel
            //               {
            //                   Page = page,
            //                   Sort = sk,
            //                   SortDirection = asc ? "asc" : "desc"
            //               };
            //foreach ( var s in allSorts )
            //{
            //    var msg = "Precompiling: VirtualResponse - order by " + (s.Sort.NullOrEmpty() ? "[default]" : s.Sort) + " " + s.SortDirection + ", page " + s.Page;
            //    Log.Info( msg );
            //    ctr.ResponsesListModel( ctx, s );
            //    Log.Info( msg + "    (done)" );
            //}
        }

        //private int _initialized = 0;
        [Import]
        public ICompositionScopingService Scoping { get; set; }
        [Import]
        public ILog Log { get; set; }
        public void Init(HttpApplication context)
        {
            //if ( Interlocked.Exchange( ref _initialized, 1 ) == 1 ) return;

            //ThreadPool.QueueUserWorkItem( _ =>
            //{
            //    try
            //    {
            //        using ( var scope = Scoping.OpenScope( TransactionScope.Id ) )
            //        {
            //            Log.Info( "Query Precompilation starting" );
            //            scope.ComposeExportedValue<HttpContextBase>( new Ctx() );
            //            scope.ComposeExportedValue<IAuthenticationService>( new Auth( scope.Get<IRepository<Model.DnsDomain, Model.User>>().All.FirstOrDefault() ) );
            //            RunQueries( scope );
            //            Log.Info( "Query Precompilation done" );
            //        }
            //    }
            //    catch ( Exception ex )
            //    {
            //        Log.Error( ex );
            //    }
            //} );
        }
        public void Dispose() { }

        class Ctx : HttpContextBase { }
        class Auth : IAuthenticationService
        {
            private readonly User _user;
            private readonly Lpp.Utilities.Security.ApiIdentity _apiIdentity;
            
            public Auth(User u) { 
                _user = u;
                _apiIdentity = new Utilities.Security.ApiIdentity(u.ID, u.UserName, u.FullName, u.OrganizationID.Value);
            }
            
            public User CurrentUser
            {
                get { return _user; }
            }

            public Lpp.Utilities.Security.ApiIdentity ApiIdentity
            {
                get { return _apiIdentity; }
            }

            public Guid CurrentUserID
            {
                get { return _user.ID; }
            }

            public void SetCurrentUser(User user, AuthenticationScope scope) { }

            
        }
    }
}