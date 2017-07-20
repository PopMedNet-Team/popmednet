using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using Lpp.Mvc;

namespace Lpp.Auth.Basic
{
    [Export( typeof( IMvcFilter ) )]
    class AuthenticationFilter<TUser, TRolesEnum> : IActionFilter, IMvcFilter, IExceptionFilter
        where TUser : class, IRoleBasedUser<TRolesEnum>
    {
        [Import] public RoleBasedAuthConfig<TRolesEnum> Config { get; set; }

        public void OnActionExecuting( ActionExecutingContext filterContext )
        {
            var u = filterContext.HttpContext.CurrentUser<TUser>();
            var userRoles = u == null ? 0 : Convert.ToInt32( u.Roles );
            if ( (from a in filterContext.ActionDescriptor.GetAttributes<RequiredRoleAttribute>()
                  let r = Convert.ToInt32( a.Roles )
                  let intersectingRoles = r & userRoles
                  let deny = a.RequireAll ? ( intersectingRoles != r ) : ( intersectingRoles == 0 )
                  where deny
                  select 0)
                 .Any()
            )
            {
                if ( filterContext.IsChildAction ) filterContext.Result = new EmptyResult();
                else filterContext.Result = Config.UnauthorizedResult( filterContext );
            }
        }

        public void OnException( ExceptionContext filterContext )
        {
            if ( !(filterContext.Exception is UnauthorizedAccessException) ) return;

            filterContext.Result = Config.UnauthorizedResult( filterContext );
            filterContext.ExceptionHandled = true;
        }

        public bool AllowMultiple { get { return true; } }
        public int Order { get { return 0; } }
        public void OnActionExecuted( ActionExecutedContext _ ) { }
    }
}