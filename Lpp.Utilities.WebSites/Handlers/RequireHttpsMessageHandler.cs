using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lpp.Utilities.WebSites.Handlers
{
    public class RequireHttpsMessageHandler : DelegatingHandler
    {

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var address = System.Web.HttpContext.Current.Request.UserHostAddress;
            if (!request.IsLocal() && request.RequestUri.Scheme != Uri.UriSchemeHttps &&
                !(address != null && 
                (address.StartsWith("192.168.") || address.StartsWith("10.") || address.StartsWith("127.0.0."))
                )               
                )
            {

                var forbiddenResponse = request.CreateResponse(HttpStatusCode.Forbidden);
                forbiddenResponse.ReasonPhrase = "HTTPS/SSL Required";
                return Task.FromResult<HttpResponseMessage>(forbiddenResponse);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
