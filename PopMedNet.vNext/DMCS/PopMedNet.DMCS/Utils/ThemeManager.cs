using Microsoft.Extensions.Options;
using PopMedNet.DMCS.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PopMedNet.DMCS
{
    public interface IThemeManager
    {
        string Title { get; }
        string Info { get; }
        string ContactUsHref { get; }
        string Terms { get; }
    }
    public class ThemeManager : IThemeManager
    {
        readonly IOptions<DMCSConfiguration> _config;
        Models.ThemeDTO _theme = null;

        public ThemeManager(IOptions<DMCSConfiguration> config)
        {
            this._config = config;
        }

        void Initialize()
        {
            using (var api = new PMNApi.PMNApiClient(_config.Value.PopMedNet.ApiServiceURL))
            {
                _theme = api.GetThemeAsync().Result;
            }
        }

        Models.ThemeDTO Theme
        {
            get
            {
                if(_theme == null)
                {
                    Initialize();  
                }

                return _theme;
            }
        }

        public string Title
        {
            get { return Theme.Title; }
        }

        public string Info
        {
            get { return Theme.Info; }
        }

        public string ContactUsHref
        {
            get { return Theme.ContactUsHref; }
        }

        public string Terms
        {
            get { return Theme.Terms; }
        }
    }
}
