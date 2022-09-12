using Lpp.Dns.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using Lpp.Utilities.WebSites.Attributes;

namespace Lpp.Dns.Api.Themes
{
    /// <summary>
    /// Controller that supports the theme
    /// </summary>
    [AllowAnonymous]
    public class ThemeController : ApiController
    {
        static System.Resources.ResourceManager _resourceManager;

        static ThemeController()
        {
            var themeID = string.IsNullOrWhiteSpace(WebConfigurationManager.AppSettings["CurrentTheme"]) ? "Default" : WebConfigurationManager.AppSettings["CurrentTheme"];

            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name == "Theme." + themeID).FirstOrDefault();
            _resourceManager = new System.Resources.ResourceManager("Theme." + themeID + ".Theme", assembly);
        }

        /// <summary>
        /// Get the theme 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, CachingFilter( TimeDuration = 10 )]
        public ThemeDTO GetText([FromUri] IEnumerable<string> keys)
        {
            ThemeDTO theme = new ThemeDTO();
            Type type = theme.GetType();

            foreach (string name in keys)
            {
                string val = _resourceManager.GetString(name);
                PropertyInfo prop = type.GetProperty(name);
                prop.SetValue(theme, val);
            }

            return theme;
        }
        /// <summary>
        /// Get the image for theme
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, CachingFilter( TimeDuration = 10 )]
        public ThemeDTO GetImagePath()
        {
            ThemeDTO theme = new ThemeDTO();
            theme.LogoImage = _resourceManager.GetString("LogoImage");

            return theme;
        }
    }
}
