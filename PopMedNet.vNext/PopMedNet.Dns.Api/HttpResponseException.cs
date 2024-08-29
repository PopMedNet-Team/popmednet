using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PopMedNet.Dns.Api
{
    public class HttpResponseException : Exception
    {
        public HttpResponseException(int statusCode, object? value = null) => (StatusCode, Value) = (statusCode, value);

        public int StatusCode { get; }
        public object? Value { get; }
    }

    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is HttpResponseException httpResponseException)
            {
                context.Result = new ObjectResult(httpResponseException.Value)
                {
                    StatusCode = httpResponseException.StatusCode
                };

                context.ExceptionHandled = true;
            }
        }
    }
}
