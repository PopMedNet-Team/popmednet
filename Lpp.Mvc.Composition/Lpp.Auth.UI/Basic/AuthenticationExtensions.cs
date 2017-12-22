using System;
using System.Diagnostics.Contracts;
using System.Web;
using Lpp.Auth;
using Lpp.Composition;
using Lpp.Mvc;

namespace Lpp.Auth.Basic
{
    public static class AuthenticationExtensions
    {
        public static bool IsInRole<TUser, TRolesEnum>( this TUser user, TRolesEnum role )
            where TUser : class, IRoleBasedUser<TRolesEnum>
        {
            Contract.Requires( user != null );
            return ( Convert.ToInt32( user.Roles ) & Convert.ToInt32( role ) ) != 0;
        }

        public static TUser CurrentUser<TUser>( this HttpContextBase ctx )
            where TUser : class, IUser
        {
            Contract.Requires( ctx != null );
            return ctx.Composition().Get<IAuthenticationService<TUser>>().CurrentUser;
        }
    }
}