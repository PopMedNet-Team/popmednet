using Lpp.Composition.Modules;
using Lpp.Mvc;

namespace Lpp.Audit.UI
{
    public static class AuditUI
    {
        public static IModule Module<TDomain>()
        {
            return new ModuleBuilder()
                .Export<IAuditUIService<TDomain>, AuditUIService<TDomain>>()
                .Export<IRouteRegistrar, Routes>()
                .CreateModule();
        }

        class Routes : IRouteRegistrar
        {
            public void RegisterRoutes( System.Web.Routing.RouteCollection routes )
            {
                routes.MapResources( GetType().Assembly );
            }

            public void RegisterCatchAllRoutes( System.Web.Routing.RouteCollection routes )
            {
            }
        }
    }
}