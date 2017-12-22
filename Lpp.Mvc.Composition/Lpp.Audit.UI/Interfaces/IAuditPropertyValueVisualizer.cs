using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Lpp.Audit.Data;

namespace Lpp.Audit.UI
{
    public interface IAuditPropertyValueVisualizer
    {
        IEnumerable<IAuditProperty> AppliesToProperties { get; }
        Func<HtmlHelper, IHtmlString> Visualize( AuditPropertyValue v );
    }
}