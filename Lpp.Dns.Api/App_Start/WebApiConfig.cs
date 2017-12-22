using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Lpp.Dns.Api
{
#pragma warning disable 1591
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {            

            // Web API routes by attributes
            //config.MapHttpAttributeRoutes();

            //Default route
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
#pragma warning restore 1591
}
