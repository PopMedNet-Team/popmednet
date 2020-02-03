using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Api
{
    public class ApplicationPreload : System.Web.Hosting.IProcessHostPreloadClient
    {
        public void Preload(string[] parameters)
        {
            HangfireBootstrapper.Instance.Start();
        }
    }
}