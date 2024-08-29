using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using Lpp.Composition;
//using Lpp.Data;
using Lpp.Dns.Model;
//using Lpp.Dns.Portal;
using Lpp.Mvc;
//using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Cchmc
{
    [Export( typeof( IMvcFilter ) )]
    public class AuthenticationFilter : IAuthorizationFilter, IMvcFilter
    {
        public void OnAuthorization( AuthorizationContext filterContext )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var res = from ctx in Maybe.Value( filterContext.HttpContext )
            //          from un in ctx.Request.Headers["OBLIX_UID"]
            //          let comp = ctx.Composition()
            //          let users = comp.Get<IRepository<DnsDomain, Model.User>>()
            //          from user in users.All.FirstOrDefault( u => u.Username == un )
            //          where !user.EffectiveIsDeleted
            //          select user;

            //if (res.Kind != MaybeKind.Value) filterContext.Result = new HttpUnauthorizedResult();
            //else filterContext.HttpContext.User = new DnsPrincipal( res.Value );
        }

        public bool AllowMultiple
        {
            get { return true; }
        }

        public int Order
        {
            get { return -10; } // The "standard" DNS auth filter has order of zero. 
                                // Therefore, in order to be called before the standard filter, I must set a negative value here.
        }
    }
}