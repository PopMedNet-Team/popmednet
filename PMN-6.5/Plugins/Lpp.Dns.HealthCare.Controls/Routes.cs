using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;

namespace Lpp.Dns.HealthCare
{
    [Export( typeof( IRouteRegistrar ) )]
    class Routes : IRouteRegistrar
    {
        public void RegisterRoutes( RouteCollection routes )
        {
            routes.MapRouteFor<Lpp.Dns.HealthCare.Controllers.FileUploadController>("DeleteFTPFile", new { action = "DeleteFTPFile" });
            routes.MapRouteFor<Lpp.Dns.HealthCare.Controllers.FileUploadController>("UploadFiles", new { action = "UploadFiles" });
            routes.MapRouteFor<Lpp.Dns.HealthCare.Controllers.FileUploadController>("VerifyFTPCredentials", new { action = "VerifyFTPCredentials" });
            routes.MapRouteFor<Lpp.Dns.HealthCare.Controllers.FileUploadController>("GetFTPPathContents", new { action = "GetFTPPathContents" });
            routes.MapRouteFor<Lpp.Dns.HealthCare.Controllers.FileUploadController>("LoadFTPFiles", new { action = "LoadFTPFiles" }); 
            routes.MapResources(this.GetType().Assembly);
        }

        public void RegisterCatchAllRoutes( RouteCollection routes )
        {
        }
    }
}