using System;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Lpp.Utilities.WebSites.Filters
{
    public class FeatureFlagFilter : ActionFilterAttribute
    {
        public string FeatureName { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if(this.FeatureName.IsEmpty())
            {
                throw new Exception("The Feaure Name has not been filled out.");
            }

            var featureValue = WebConfigurationManager.AppSettings[this.FeatureName] == null ? false : WebConfigurationManager.AppSettings[this.FeatureName].ToBool();

            if (!featureValue)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
