using Lpp.Dns.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lpp.Dns.Portal
{
    public static class SharedDataContextExtensions
    {
        public static DataContext DataContext(this HttpContext http)
        {
            return http.Items["DataContext"] as DataContext;
        }

        public static DataContext DataContext(this HttpContextBase http)
        {
            return http.Items["DataContext"] as DataContext;
        }
    }
}
