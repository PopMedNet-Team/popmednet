using System;
using System.Web.Mvc;

namespace Lpp.Auth.Basic
{
    class RoleBasedAuthConfig<TRolesEnum>
    {
        public TRolesEnum FirstUserRole { get; set; }
        public Func<ControllerContext,ActionResult> UnauthorizedResult { get; set; }
    }
}