using System;
using System.ComponentModel.Composition;
using System.Web;
using System.Web.Security;
using Lpp.Composition;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal
{
    [Export(typeof(IAuthenticationService))]
    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    public class AuthenticationService : IAuthenticationService
    {
        [Import]
        public HttpContextBase HttpContext { get; set; }

        public User CurrentUser
        {
            get
            {
                var p = HttpContext.User as DnsPrincipal;
                return p == null ? null : p.User;
            }
        }

        public Lpp.Utilities.Security.ApiIdentity ApiIdentity
        {
            get
            {
                var p = HttpContext.User as DnsPrincipal;
                return p == null ? null : p.ApiIdentity;
            }
        }

        public Guid CurrentUserID
        {
            get
            {
                var p = HttpContext.User as DnsPrincipal;
                return p == null || p.User == null ? Guid.Empty : p.User.ID;
            }
        }

        public void SetCurrentUser(User user, AuthenticationScope scope)
        {
            if (user == null)
            {
                HttpContext.User = null;
                if (scope != AuthenticationScope.Transaction) FormsAuthentication.SignOut();
                return;
            }

            HttpContext.User = new DnsPrincipal(user);
            if (scope != AuthenticationScope.Transaction)
            {
                FormsAuthentication.SetAuthCookie(user.ID.ToString(), scope == AuthenticationScope.Permanent);
            }
        }
    }
}