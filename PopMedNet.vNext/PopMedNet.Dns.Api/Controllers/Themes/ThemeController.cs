using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.Serialization;
using IO = System.IO;

namespace PopMedNet.Dns.Api.Themes
{

    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class ThemeController : ControllerBase
    {
        readonly string _themeRootPath;

        public ThemeController(IWebHostEnvironment env, IConfiguration configuration)
        {
            _themeRootPath = IO.Path.Combine(env.ContentRootPath, "themes", string.IsNullOrWhiteSpace(configuration["appSettings:CurrentTheme"]) ? "Default" : configuration["appSettings:CurrentTheme"]);
        }

        /// <summary>
        /// Get the theme 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        [HttpGet("gettext")]
        public async Task<ActionResult<ThemeDTO>> GetText([FromQuery] IEnumerable<string> keys)
        {
            ThemeDTO theme = new ThemeDTO();
            foreach (string key in keys)
            {
                await PopulateValue(theme, key.ToLower());
            }

            return theme;
        }
        /// <summary>
        /// Get the image for theme
        /// </summary>
        /// <returns></returns>
        [HttpGet("getimagepath")]
        public async Task<ActionResult<ThemeDTO>> GetImagePath()
        {
            ThemeDTO theme = new ThemeDTO();
            await PopulateValue(theme, "logoimage");

            return theme;
        }

        async Task PopulateValue(ThemeDTO theme, string key)
        {
            string path = IO.Path.Combine(_themeRootPath, key + ".html");
            if (IO.File.Exists(path))
            {
                switch (key)
                {
                    case "title":
                        theme.Title = await IO.File.ReadAllTextAsync(path);
                        break;
                    case "terms":
                        theme.Terms = await IO.File.ReadAllTextAsync(path);
                        break;
                    case "info":
                        theme.Info = await IO.File.ReadAllTextAsync(path);
                        break;
                    case "resources":
                        theme.Resources = await IO.File.ReadAllTextAsync(path);
                        break;
                    case "footer":
                        theme.Footer = await IO.File.ReadAllTextAsync(path);
                        break;
                    case "logoimage":
                        theme.LogoImage = await IO.File.ReadAllTextAsync(path);
                        break;
                    case "systemuserconfirmationtitle":
                        theme.SystemUserConfirmationTitle = await IO.File.ReadAllTextAsync(path);
                        break;
                    case "systemuserconfirmationcontent":
                        theme.SystemUserConfirmationContent = await IO.File.ReadAllTextAsync(path);
                        break;
                    case "contactushref":
                        theme.ContactUsHref = await IO.File.ReadAllTextAsync(path);
                        break;
                }
            }
        }
    }
    [DataContract]
    public class ThemeDTO
    {
        /// <summary>
        /// Title
        /// </summary>
        [DataMember]
        public string? Title { get; set; }
        /// <summary>
        /// Terms
        /// </summary>
        [DataMember]
        public string? Terms { get; set; }
        /// <summary>
        /// Info
        /// </summary>
        [DataMember]
        public string? Info { get; set; }
        /// <summary>
        /// Resources
        /// </summary>
        [DataMember]
        public string? Resources { get; set; }
        /// <summary>
        /// Footer
        /// </summary>
        [DataMember]
        public string? Footer { get; set; }
        /// <summary>
        /// Logo image
        /// </summary>
        [DataMember]
        public string? LogoImage { get; set; }

        /// <summary>
        /// SystemUserConfirmationTitle
        /// </summary>
        [DataMember]
        public string? SystemUserConfirmationTitle { get; set; }

        /// <summary>
        /// SystemUserConfirmationContent
        /// </summary>
        [DataMember]
        public string? SystemUserConfirmationContent { get; set; }

        /// <summary>
        /// Contact Us Link
        /// </summary>
        [DataMember]
        public string? ContactUsHref { get; set; }
    }

}
