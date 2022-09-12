using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.WebSites.Models;

namespace Lpp.Dns.Portal.Controllers
{
    public class UsersController : BaseController
    {
        Lpp.Utilities.Security.ApiIdentity ApiIdentity
        {
            get
            {

                var apiIdentity = HttpContext.User.Identity as Lpp.Utilities.Security.ApiIdentity;
                if (apiIdentity == null)
                    throw new System.Security.SecurityException("User is not logged in.");

                return apiIdentity;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Details()
        {
            Guid userID;
            if (Guid.TryParse((Request.QueryString["ID"] ?? ApiIdentity.ID.ToString()), out userID))
            {

                Lpp.Dns.Data.ExtendedQuery query = new Lpp.Dns.Data.ExtendedQuery() { Users = a => a.UserID == userID };

                var permissionsList = new List<PermissionDefinition>();
                permissionsList.Add(DTO.Security.PermissionIdentifiers.User.ChangePassword);

                var grantedPermissions = await DataContext.HasGrantedPermissions(ApiIdentity, query, permissionsList.ToArray());

                ViewBag.ScreenPermissions = grantedPermissions.Select(p => p.ID).ToList();
            }

            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult RejectRegistration()
        {
            return View();
        }

        public ActionResult Deactivate()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SendEmail()
        {
            List<Tuple<int, string, string>> templates = new List<Tuple<int, string, string>>();
            var section = System.Web.Configuration.WebConfigurationManager.GetSection("userEmailTemplates") as Lpp.Utilities.WebSites.Configuration.UserEmailTemplateSection;
            if (section != null)
            {
                for (int i = 0; i < section.Templates.Count; i++) {
                    var template = section.Templates[i];
                    templates.Add(new Tuple<int, string, string>(template.TemplateTypeID, template.TemplateName, template.Subject));
                }
            }

            return View(templates);
        }

        [HttpPost]
        public async Task<ActionResult> SendEmail(Guid userID, int templateType)
        {
            var section = System.Web.Configuration.WebConfigurationManager.GetSection("userEmailTemplates") as Lpp.Utilities.WebSites.Configuration.UserEmailTemplateSection;
            if (section == null)
            {
                return HttpNotFound("Could not find the configuration section for user email templates.");
            }

            var template = section.Templates.FindByTemplateID(templateType);

            if (template == null)
            {
                return HttpNotFound($"Could not find the email template with the template type ID of {templateType}");
            }

            var user = DataContext.Users.Where(u => u.ID == userID).Select(u => u).FirstOrDefault();

            if(user == null)
            {
                return HttpNotFound("Could not find the specified user to send the email to.");
            }

            var cookie = Request.Cookies.Get("Authorization").Value;
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponseModel>(cookie);

            string username;
            string password;
            LoginResponseModel.DecryptCredentials(model.Authorization, out username, out password);
            using (var api = new ApiClient.DnsClient(System.Web.Configuration.WebConfigurationManager.AppSettings["ServiceUrl"], username, password))
            {
                var details = new DTO.Users.EmailTemplateSubstitutionPropertiesDTO
                {
                    UserID = user.ID,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    Username = user.UserName,
                    EmailAddress = user.Email,
                    TemplateTypeID = templateType,
                    Subject = template.Subject,
                    Network = DataContext.Networks.Where(n => n.Name.Equals("Aqueduct", StringComparison.OrdinalIgnoreCase) == false).Select(n => n.Name).FirstOrDefault() ?? "Default",
                    QueryToolName = section.QueryToolName
                };

                string siteUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["SsoUrl"];
                if (string.IsNullOrEmpty(siteUrl))
                {
                    if (Request.Url.IsDefaultPort)
                    {
                        siteUrl = new UriBuilder(Request.Url.Scheme, Request.Url.Host).ToString();
                    }
                    else
                    {
                        siteUrl = new UriBuilder(Request.Url.Scheme, Request.Url.Host, Request.Url.Port).ToString();
                    }                    
                }
                details.NetworkUrl = siteUrl;
                details.ApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ServiceUrl"];

                await api.Users.SendEmail(new DTO.Users.SendEmailRequest
                {
                    Properties = details,
                    HtmlContent = RenderRazorViewToString(template.HtmlTemplatePath, details),
                    TextContent = RenderRazorViewToString(template.TextTemplatePath, details)
                });
            }

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public async Task<ActionResult> RenderEmailPreview(Guid userID, int? templateType = null)
        {
            if(templateType == null)
            {
                return View("~/Views/Users/Notifications/_SelectTemplate.cshtml");
            }

            var section = System.Web.Configuration.WebConfigurationManager.GetSection("userEmailTemplates") as Lpp.Utilities.WebSites.Configuration.UserEmailTemplateSection;
            if(section == null)
            {
                return HttpNotFound("Could not find the configuration section for user email templates.");
            }
             
            var template = section.Templates.FindByTemplateID(templateType.Value);

            if(template == null)
            {
                return HttpNotFound($"Could not find the email template with the template type ID of { templateType.Value }");
            }

            var user = DataContext.Users.Where(u => u.ID == userID).Select(u => u).FirstOrDefault();

            var details = new DTO.Users.EmailTemplateSubstitutionPropertiesDTO
            {
                UserID = user.ID,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                FullName = user.FullName,
                Username = user.UserName,
                EmailAddress = user.Email,
                TemplateTypeID = templateType.Value,
                Subject = template.Subject,
                Network = DataContext.Networks.Where(n => n.Name.Equals("Aqueduct", StringComparison.OrdinalIgnoreCase) == false).Select(n => n.Name).FirstOrDefault() ?? "Default",
                QueryToolName = section.QueryToolName
            };

            string siteUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["SsoUrl"];
            if (string.IsNullOrEmpty(siteUrl))
            {
                siteUrl = new UriBuilder(Request.Url.Scheme, Request.Url.Host, Request.Url.Port).ToString();
            }
            details.NetworkUrl = siteUrl;
            details.ApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ServiceUrl"];

            return View(template.HtmlTemplatePath, details);
        }



    }
}