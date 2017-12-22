using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web.WebPages.Razor;
using System.Web.Hosting;
using System.Collections.Concurrent;
using System.Web.WebPages;

namespace Lpp.Mvc
{
    public interface IThemeService
    {
        string CurrentTheme { get; set; }
    }
}