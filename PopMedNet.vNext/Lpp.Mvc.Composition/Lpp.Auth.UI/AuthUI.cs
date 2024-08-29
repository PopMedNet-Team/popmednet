using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Lpp.Auth.UI;
using Lpp.Composition;
using Lpp.Composition.Modules;
using Lpp.Mvc;

namespace Lpp.Auth
{
    public static class AuthUI
    {
        public static AuthUIModule<TDomain, TUser, TView> Module<TDomain, TUser, TView>(
            Func<ICompositionService, IAuthResult, TUser> findUser,
            Func<ICompositionService, IAuthResult, TUser> findOrCreateUser,
            params IAuthProviderDefinition[] providers )
            where TUser : class, IUser, new()
            where TView : WebViewPage<LoginModel>
        {
            Contract.Requires( findUser != null );
            Contract.Requires( findOrCreateUser != null );
            Contract.Ensures( Contract.Result<AuthUIModule<TDomain, TUser, TView>>() != null );
            return new AuthUIModule<TDomain, TUser, TView>( findUser, findOrCreateUser, providers );
        }

        public static AuthUIModule<TDomain, TUser, TView> Module<TDomain, TUser, TView>( params IAuthProviderDefinition[] providers )
            where TUser : class, IUser, new()
            where TView : WebViewPage<LoginModel>
        {
            Contract.Ensures( Contract.Result<AuthUIModule<TDomain, TUser, TView>>() != null );
            return Module<TDomain, TUser, TView>( 
                ( s, r ) => s.Get<IUserProvider<TUser>>().GetByClaimedId( r.UserId, false ),
                ( s, r ) => s.Get<IUserProvider<TUser>>().GetByClaimedId( r.UserId, true ), 
                providers );
        }

        public static void MapRoutes( RouteCollection routes, string login = null, string logout = null, string loginPost = null )
        {
            Contract.Requires( routes != null );
            routes.MapRouteFor<UI.AuthController>( login ?? "login", new { action = "Login" } );
            routes.MapRouteFor<UI.AuthController>( loginPost ?? "loginpost", new { action = "DoLogin" } );
            routes.MapRouteFor<UI.AuthController>( logout ?? "logout", new { action = "Logout" } );
            routes.MapResources( typeof( AuthUI ).Assembly );
        }
    }

    public class AuthUIModule<TDomain, TUser, TView> : IModule
        where TUser : class, IUser, new()
        where TView : WebViewPage<LoginModel>
    {
        private readonly Func<ICompositionService, IAuthResult, TUser> _findUser;
        private readonly Func<ICompositionService, IAuthResult, TUser> _findOrCreateUser;
        private readonly IEnumerable<Action<ModuleBuilder>> _configs;
        private readonly IEnumerable<IAuthProviderDefinition> _providers;

        public IEnumerable<ModuleDefinition> GetDefinition()
        {
            var m = new ModuleBuilder()
                .Export<IAuthUIService, AuthUIService<TDomain, TUser, TView>>()
                .Export( new AuthUIConfig<TDomain, TUser> { FindOrCreateUser = _findOrCreateUser, FindUser = _findUser } );
            foreach ( var c in _configs.EmptyIfNull() ) c( m );
            foreach ( var p in _providers.EmptyIfNull().Reverse() ) m = m.Export( p );
            return m.CreateModule().GetDefinition();
        }

        public AuthUIModule<TDomain, TUser, TView> ConfigRequest<T>( IConfigAuthRequest<T> c )
        {
            Contract.Requires( c != null );
            return new AuthUIModule<TDomain, TUser, TView>( _findUser, _findOrCreateUser, _providers,
                _configs.EmptyIfNull().StartWith( m => m.Export( c ) ) );
        }

        public AuthUIModule<TDomain, TUser, TView> ConfigRequest<T>( Action<T> c )
        {
            Contract.Requires( c != null );
            return this.ConfigRequest( new AnonymousConfig<T>( c ) );
        }

        internal AuthUIModule(
            Func<ICompositionService, IAuthResult, TUser> findUser,
            Func<ICompositionService, IAuthResult, TUser> findOrCreateUser,
            IEnumerable<IAuthProviderDefinition> providers,
            IEnumerable<Action<ModuleBuilder>> configs = null)
        {
            _findUser = findUser;
            _findOrCreateUser = findOrCreateUser;
            _providers = providers.EmptyIfNull().Any() ? providers : AuthProviders.AllOpenId;
            _configs = configs;
        }

        class AnonymousConfig<T> : IConfigAuthRequest<T>
        {
            private readonly Action<T> _config;
            public AnonymousConfig( Action<T> c ) { _config = c; }
            public void Config( T r ) { _config( r ); }
        }
    }

    class AuthUIConfig<TDomain, TUser>
    {
        public Func<ICompositionService, IAuthResult, TUser> FindUser { get; set; }
        public Func<ICompositionService, IAuthResult, TUser> FindOrCreateUser { get; set; }
    }
}