using System;
using System.Web.Http;
using System.Web.Mvc;
using Lpp.Dns.Api.Areas.HelpPage.ModelDescriptions;
using Lpp.Dns.Api.Areas.HelpPage.Models;
using Lpp.Utilities.WebSites.Attributes;
using Lpp.Dns.WebServices;

namespace Lpp.Dns.Api.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    [BasicAuthenticationMVC]
    public class HelpController : Controller
    {
        private const string ErrorViewName = "Error";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public HelpController()
            : this(GlobalConfiguration.Configuration)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config"></param>
        public HelpController(HttpConfiguration config)
        {
            Configuration = config;
        }

        /// <summary>
        /// Help page configuration.
        /// </summary>
        public HttpConfiguration Configuration { get; private set; }

        /// <summary>
        /// Default help action.
        /// </summary>
        /// <returns>API descriptions</returns>
        public ActionResult Index()
        {
            ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
            return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

        /// <summary>
        /// Action to return help page for specified API id.
        /// </summary>
        /// <param name="apiId">API id</param>
        /// <returns>Specified API help page</returns>
        public ActionResult Api(string apiId)
        {
            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return View(ErrorViewName);
        }

        /// <summary>
        /// Action to return resource model description.
        /// </summary>
        /// <param name="modelName">Name of the model</param>
        /// <returns>Model description</returns>
        public ActionResult ResourceModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return View(ErrorViewName);
        }
    }
}