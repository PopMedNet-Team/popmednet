using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using System.Configuration;
using System.Globalization;
using System.ComponentModel.Composition;
using Lpp.Composition;
using System.Web;

namespace Lpp.Mvc.Application
{
    /// <summary>
    /// Default implementation if <see cref="IThemeService"/> - simply stores theme information in the Session
    /// </summary>
    public class ThemeService : IThemeService
    {
        readonly HttpContextBase _context;
        const string ThemeKey = "CurrentTheme_{F5AC9F30-8958-4884-95B0-73535F14ECEC}";

        [ImportingConstructor]
        public ThemeService( [Import] HttpContextBase context )
	    {
            //Contract.Requires(context != null);
            _context = context;
	    }

        public string CurrentTheme
        {
            get { return _context.Session[ThemeKey] as string ?? ""; }
            set { _context.Session[ThemeKey] = value; }
        }
    }
}