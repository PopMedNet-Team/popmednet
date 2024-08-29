﻿using Lpp.Dns.DTO;
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

namespace Lpp.Dns.Api.Themes
{
    /// <summary>
    /// Controller that supports the theme
    /// </summary>
    [AllowAnonymous]
    public class ThemeController : ApiController
    {
        /// <summary>
        /// Get the theme 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ThemeDTO GetText([FromUri] IEnumerable<string> keys)
        {
            var themeID = string.IsNullOrWhiteSpace(WebConfigurationManager.AppSettings["CurrentTheme"]) ? "Default" : WebConfigurationManager.AppSettings["CurrentTheme"];

            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name == "Theme." + themeID).FirstOrDefault();
            System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager("Theme." + themeID + ".Theme", assembly);


            ThemeDTO theme = new ThemeDTO();
            Type type = theme.GetType();

            foreach (string name in keys)
            {
                string val = resourceManager.GetString(name);
                PropertyInfo prop = type.GetProperty(name);
                prop.SetValue(theme, val);
            }

            return theme;
        }
        /// <summary>
        /// Get the image for theme
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ThemeDTO GetImagePath()
        {
            var themeID = string.IsNullOrWhiteSpace(WebConfigurationManager.AppSettings["CurrentTheme"]) ? "Default" : WebConfigurationManager.AppSettings["CurrentTheme"];

            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name == "Theme." + themeID).FirstOrDefault();
            System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager("Theme." + themeID + ".Theme", assembly);

            ThemeDTO theme = new ThemeDTO();
            theme.LogoImage = resourceManager.GetString("LogoImage");

            return theme;
        }
    }
}
