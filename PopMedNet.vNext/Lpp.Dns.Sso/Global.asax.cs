﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Lpp.Dns.Sso
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static readonly string ApplicationVersion;

        static MvcApplication()
        {
            ApplicationVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(MvcApplication).Assembly.Location).ProductVersion;
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


        }
    }
}
