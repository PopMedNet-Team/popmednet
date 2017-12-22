using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web.WebPages.Razor;
using System.Web.Hosting;

namespace Lpp.Mvc
{
    public class CompiledViewsAttribute : Attribute
    {
        public string DefaultNamespace { get; set; }
        public bool AllowOverride { get; set; }

        public CompiledViewsAttribute()
        {
        }

        public CompiledViewsAttribute( Type exampleViewType )
        {
            DefaultNamespace = exampleViewType.Namespace;
        }
    }
}