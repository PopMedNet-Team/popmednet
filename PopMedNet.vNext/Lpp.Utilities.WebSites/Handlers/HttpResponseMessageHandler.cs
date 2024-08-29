using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities.WebSites.Handlers
{
    public class HttpResponseMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {


            var response = await base.SendAsync(request, cancellationToken);

            if (response.Content == null && request.Headers.Accept.Any(a => a.MediaType == "application/json"))
            {
                var content = new StringContent("{}");
                content.Headers.ContentLength = 2;
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                response.Content = content;
            }

            return response;
        }
    }
}
