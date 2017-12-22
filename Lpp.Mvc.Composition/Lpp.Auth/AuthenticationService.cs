using System;
using System.ComponentModel.Composition;
using System.Web;
using System.Web.Security;
using Lpp.Composition;

namespace Lpp.Auth
{
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    class AuthenticationService<TUser> : IAuthenticationService<TUser>
         where TUser : class, IUser
    {
        [Import] public HttpContextBase HttpContext { get; set; }

        public TUser CurrentUser
        {
            get
            {
                var p = HttpContext.User as Principal<TUser>;
                return p == null ? null : p.User;
            }
        }

        public void SetCurrentUser( TUser user, AuthenticationScope scope )
        {
            if ( user == null )
            {
                HttpContext.User = null;

                if ( scope != AuthenticationScope.Transaction ) 
                    FormsAuthentication.SignOut();

                return;
            }

            HttpContext.User = new Principal<TUser>( user );

            if ( scope != AuthenticationScope.Transaction )
                FormsAuthentication.SetAuthCookie( user.Id ?? "", scope == AuthenticationScope.Permanent );
        }
    }
}