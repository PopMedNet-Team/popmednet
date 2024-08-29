using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PopMedNet.Dns.Portal.Code
{
    public class CommonPageValuesActionFilter : IActionFilter
    {
        readonly string? _serviceUrl;
        readonly int? _sessionDurationMinutes;
        readonly int? _sessionWarningMinutes;

        public CommonPageValuesActionFilter(IConfiguration config)
        {
            _serviceUrl = config["ServiceUrl"];
            _sessionDurationMinutes = config.GetValue<int?>("SessionExpirationMinutes");
            _sessionWarningMinutes = config.GetValue<int?>("SessionWarningMinutes");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Controller? controller = context.Controller as Controller;
            if (controller != null)
            {
                controller.ViewData["ServiceUrl"] = _serviceUrl;
                controller.ViewData["SessionDurationMinutes"] = _sessionDurationMinutes;
                controller.ViewData["SessionWarningMinutes"] = _sessionWarningMinutes;
            }
        }
    }
}
