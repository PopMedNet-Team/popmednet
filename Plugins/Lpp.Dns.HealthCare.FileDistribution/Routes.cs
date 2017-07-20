using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;

namespace Lpp.Dns.Models.FileDistribution
{
    [Export(typeof(IRouteRegistrar))]
    class Routes : IRouteRegistrar
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRouteFor<Lpp.Dns.HealthCare.Controllers.FileDistributionResultController>("DownloadFile", new { action = "Download" });
            routes.MapResources(this.GetType().Assembly);
        }

        public void RegisterCatchAllRoutes(RouteCollection routes)
        {
        }
    }
}