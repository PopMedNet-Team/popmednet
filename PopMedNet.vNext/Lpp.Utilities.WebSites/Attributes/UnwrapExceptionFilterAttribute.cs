using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Http;

namespace Lpp.Utilities.WebSites.Attributes
{
    public class UnwrapExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is HttpResponseException)
                return;

            var exceptionString = actionExecutedContext.Exception.UnwindException(false);

#if(DEBUG)            
            exceptionString += "\r\n\r\nStack Trace: \r\n\r\n" + actionExecutedContext.Exception.StackTrace;
#endif

            actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, exceptionString);
        }
    }
}
