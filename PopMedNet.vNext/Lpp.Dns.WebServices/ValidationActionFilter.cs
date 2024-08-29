using Lpp.Dns.DTO;
using Lpp.Objects;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Lpp.Dns.WebServices
{
    /// <summary>
    /// Filters all requests to the API and returns validation errors as friendly error messages.
    /// </summary>
    public class ValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.Request.Method != HttpMethod.Get)
            {
                if (!actionContext.ModelState.IsValid)
                {

                    var errors = actionContext.ModelState
                        .Where(s => s.Value.Errors.Count > 0)
                        .Select(s => new KeyValuePair<string, string>(s.Key, s.Value.Errors.First().ErrorMessage))
                        .ToArray();

                    var response = new BaseResponse<EntityDto>();
                    response.results = null;
                    response.errors = actionContext.ModelState.Where(s => s.Value.Errors.Any())
                        .Select(s => new ResponseError
                        {
                            Description = s.Value.Errors.First().ErrorMessage,
                            ErrorType = s.Value.Errors.First().Exception != null ? s.Value.Errors.First().Exception.GetType().Name : "N/A",
                            Property = s.Key
                        })
                        .ToArray();

                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }
            }
        }
    }

}
