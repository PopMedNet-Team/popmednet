using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Lpp.Utilities.WebSites.Attributes
{
    public class CachingFilter : ActionFilterAttribute
    {
        public double TimeDuration { get; set; }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
            {
                MaxAge = TimeSpan.FromMinutes(TimeDuration),
                MustRevalidate = true,
                Public = true
            };
        }
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            actionExecutedContext.Response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
            {
                MaxAge = TimeSpan.FromMinutes(TimeDuration),
                MustRevalidate = true,
                Public = true
            };

            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }
    }
}
