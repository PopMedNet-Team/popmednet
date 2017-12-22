using System.Linq.Expressions;
using System;

namespace Lpp.Auth
{
    public interface IAuthenticationService<TUser>
         where TUser : class, IUser
    {
        TUser CurrentUser { get; }
        void SetCurrentUser( TUser user, AuthenticationScope scope );
    }

    public enum AuthenticationScope
    {
        Transaction,
        WebSession,
        Permanent
    }
}