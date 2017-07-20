using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Composition.Modules;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;

namespace Lpp.Security.UI
{
    public static class SecurityUI
    {
        public static IModule Module<TDomain>()
        {
            return new ModuleBuilder()
                .Export<ISecurityUIService<TDomain>, SecurityUIService<TDomain>>()
                .CreateModule();
        }
    }
}