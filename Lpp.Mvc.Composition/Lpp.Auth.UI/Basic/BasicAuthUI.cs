using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using Lpp.Auth.UI;
using Lpp.Composition;
using Lpp.Composition.Modules;
using Lpp.Data;
using Lpp.Mvc;

namespace Lpp.Auth.Basic
{
    public static class BasicAuthUI
    {
        public static IModule RoleBased<TDomain, TUser, TRolesEnum, TLoginView>( TRolesEnum firstUserRole,
            Func<ControllerContext, ActionResult> unauthorizedResult, params IAuthProviderDefinition[] providers )
            where TUser : class, IRoleBasedUser<TRolesEnum>, new()
            where TLoginView : WebViewPage<LoginModel>
        {
            Contract.Requires<ArgumentException>( typeof( TRolesEnum ).IsEnum, "Expected an enum" );
            Contract.Requires<ArgumentException>( typeof( TRolesEnum ).GetCustomAttributes( typeof( FlagsAttribute ), true ).Length > 0, "Roles enum must must be [Flags]" );

            var getUser = GetByClaimedId<TDomain, TUser, TRolesEnum>( firstUserRole, OpenIdInfo, OAuthInfo );
            return new ModuleBuilder()
                .Require( AuthUI.Module<TDomain, TUser, TLoginView>( getUser, getUser, providers )
                            .ConfigRequest( (IAuthenticationRequest r) => r.AddExtension( _stdFetchRequest ) ) )
                .Export<IUserProvider<TUser>, UserProvider<TDomain, TUser, TRolesEnum>>()
                .Export<IMvcFilter, AuthenticationFilter<TUser, TRolesEnum>>()
                .Export( new RoleBasedAuthConfig<TRolesEnum> { FirstUserRole = firstUserRole, UnauthorizedResult = unauthorizedResult } )
                .CreateModule();
        }

        public static IModule RoleBased<TDomain, TUser, TRolesEnum, TLoginView, THomeController>(
            TRolesEnum firstUserRole, Expression<Func<THomeController, object>> unauthorizedRedirectTo, params IAuthProviderDefinition[] providers )
            where TUser : class, IRoleBasedUser<TRolesEnum>, new()
            where THomeController : IController
            where TLoginView : WebViewPage<LoginModel>
        {
            return RoleBased<TDomain, TUser, TRolesEnum, TLoginView>( firstUserRole,
                ctx => new RedirectResult( new UrlHelper( ctx.HttpContext.Request.RequestContext ).Action( unauthorizedRedirectTo ) ),
                providers );
        }

        static Func<ICompositionService, IAuthResult, TUser> GetByClaimedId<TDomain, TUser, TRolesEnum>(
            TRolesEnum firstUserRole, params Func<IAuthResult, MaybeNotNull<UserInfo>>[] infos )
            where TUser : class, IRoleBasedUser<TRolesEnum>, new()
        {
            return ( c, r ) =>
            {
                var uow = c.Get<IUnitOfWork<TDomain>>();
                var repo = c.Get<IRepository<TDomain, TUser>>();
                var claimedId = r.UserId;
                var info = infos.Select( i => i( r ) ).Aggregate( (a, b) => a.OrMaybe( () => b ) ).Or( () => new UserInfo() ).Value;

                var firstUser = repo.All.Count() == 0;
                var user =
                    repo.All.FirstOrDefault( u => u.Login == claimedId ) ?? 
                    repo.Add( new TUser
                        { Login = claimedId, Roles = firstUser ? firstUserRole : default( TRolesEnum ), Name = info.Name, Email = info.Email } 
                    );
                uow.Commit();
                return user;
            };
        }

        static MaybeNotNull<UserInfo> OpenIdInfo( IAuthResult r )
        {
            return from p in Maybe.Value( r as OpenIdAuthResult )
                   from fetch in p.Response.GetExtension<FetchResponse>()
                   select new UserInfo
                   {
                       Name = fetch.GetAttributeValue( WellKnownAttributes.Name.Alias ),
                       Email = fetch.GetAttributeValue( WellKnownAttributes.Contact.Email )
                   };
        }

        static MaybeNotNull<UserInfo> OAuthInfo( IAuthResult r )
        {
            return from p in Maybe.Value( r as OAuthResult )
                   select new UserInfo
                   {
                       Name = s(p, "name") ?? s(p, "first_name") + " " + s(p, "last_name"),
                       Email = s(p, "email")
                   };
        }

        static string s( OAuthResult p, string name )
        {
            var o = p.Fields.ValueOrDefault( name );
            return o == null ? null : o.ToString();
        }

        class UserInfo { public string Name; public string Email; }

        private static readonly FetchRequest _stdFetchRequest = new FetchRequest
        {
            Attributes = { 
                new AttributeRequest( WellKnownAttributes.Contact.Email, true ),
                new AttributeRequest( WellKnownAttributes.Name.Alias, true )
            }
        };
    }
}