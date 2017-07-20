using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;
using Lpp.Composition;
using Lpp.Data;
using Lpp.Mvc;

namespace Lpp.Auth.UI
{
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    class AuthUIService<TDomain, TUser, TView> : IAuthUIService
        where TUser : class, IUser, new()
        where TView : WebViewPage<LoginModel>
    {
        [Import] public IUnitOfWork<TDomain> UnitOfWork { get; set; }
        [Import] public IRepository<TDomain, TUser> Users { get; set; }
        [Import] public IAuthenticationService<TUser> Auth { get; set; }
        [Import] public AuthUIConfig<TDomain, TUser> Config { get; set; }
        [Import] public HttpContextBase HttpContext { get; set; }
        [Import] public ICompositionService Comp { get; set; }
        [ImportMany] public IEnumerable<IAuthProviderDefinition> Providers { get; set; }
        [ImportMany] public IEnumerable<IAuthProviderHandler> Handlers { get; set; }

        public ActionResult Login( Guid? provider, string returnTo )
        {
            if ( Auth.CurrentUser != null ) return ReturnTo( returnTo );

            var u = from pid in Maybe.Value( provider )
                    where pid.HasValue
                    from resp in GetResult( pid.Value )
                    from user in Config.FindOrCreateUser( Comp, resp )
                    select user;

            if ( u.Kind == MaybeKind.Value )
            {
                Auth.SetCurrentUser( u.Value, AuthenticationScope.Permanent );
                return ReturnTo( returnTo );
            }

            var m = new LoginModel { Providers = Providers, ReturnTo = returnTo };
            m.LoginView = html => html.Partial<Views.Login>().WithModel( m );
            return View.Result<TView>().WithModel( m );
        }

        private MaybeNotNull<IAuthResult> GetResult( Guid provider )
        {
            return (from p in Providers
                    where p.Guid == provider
                    from h in Handlers
                    let r = h.GetResult( p )
                    where r != null
                    select r
                   )
                   .MaybeFirst();
        }

        private ActionResult RedirectToLogin( string returnTo )
        {
            return new RedirectResult(
                new UrlHelper( HttpContext.Request.RequestContext ).Action( ( AuthController c ) => c.Login( null, returnTo ) ) );
        }

        private ActionResult ReturnTo( string returnTo )
        {
            return new RedirectResult( returnTo ?? "~/" );
        }

        public ActionResult DoLogin( string providerGuid, string username, string returnTo )
        {
            if ( Auth.CurrentUser != null ) return ReturnTo( returnTo );

            var guid = Maybe.ParseGuid( providerGuid ).Catch().ValueOrDefault();
            var ret = new Uri( HttpContext.Request.Url, new UrlHelper( HttpContext.Request.RequestContext ).Action( (AuthController c) => c.Login( guid, returnTo ) ) );

            return EnumerableEx.Defer( () => 
                    from p in Providers
                    where p.Guid == guid
                    from h in Handlers
                    let r = h.PrepareAuthRequest( p, username, ret )
                    where r != null
                    select r
                )
                .Catch( ( Exception ex ) => Enumerable.Empty<ActionResult>() )
                .Repeat( 3 )
                .FirstOrDefault()
                ??
                RedirectToLogin( returnTo );
        }

        public ActionResult Logout( string returnTo )
        {
            Auth.SetCurrentUser( null, AuthenticationScope.Permanent );
            return ReturnTo( returnTo );
        }
    }
}