using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using Lpp;
using Lpp.Auth;
using Lpp.Composition;
using Lpp.Mvc;
using Lpp.Utilities.Legacy;

namespace Lpp.Auth
{
    class AuthenticationFilter<TUser> : IAuthorizationFilter, IMvcFilter
         where TUser : class, IUser
    {
        public void OnAuthorization( AuthorizationContext filterContext )
        {
            var _ = from comp in Maybe.Value( filterContext.HttpContext.Composition() )
                    from auth in comp.Get<IAuthenticationService<TUser>>()
                    where auth.CurrentUser == null
                    from u in Maybe.Value( filterContext.HttpContext.User )
                    from i in u.Identity
                    from n in i.Name
                    where !n.NullOrEmpty()
                    from prov in comp.Get<IUserProvider<TUser>>()
                    from user in prov.FindUser( n )
                    select Maybe.Do( () => auth.SetCurrentUser( user, AuthenticationScope.Transaction ) );
        }

        public bool AllowMultiple
        {
            get { return true; }
        }

        public int Order
        {
            get { return 0; }
        }
    }
}