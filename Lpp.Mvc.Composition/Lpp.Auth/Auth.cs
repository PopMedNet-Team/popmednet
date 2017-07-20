using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Security.Principal;
using System.Web;
using System.Threading;
using Lpp.Composition.Modules;
using System.Web.Mvc;

namespace Lpp.Auth
{
    public static class Auth
    {
        public static IModule Module<TDomain, TUser>()
            where TUser : class, IUser
        {
            return new ModuleBuilder()
                .Export<IAuthenticationService<TUser>, AuthenticationService<TUser>>()
                .Export<IMvcFilter, AuthenticationFilter<TUser>>()
                .CreateModule(); 
        }
    }
}