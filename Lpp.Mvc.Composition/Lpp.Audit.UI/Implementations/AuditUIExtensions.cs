using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc;
using Lpp.Composition;
using Lpp.Utilities.Legacy;

namespace Lpp.Audit.UI
{
    public static class AuditUIExtensions
    {
        public static IHtmlString ForAllKinds( this IAuditEventFiltersUI ui, string fieldName, 
            ILookup<AuditEventKind, IAuditEventFilter> initialState = null )
        {
            var factories = ui.Html.ViewContext.HttpContext.Composition().GetMany<IAuditEventFilterUIFactory>();
            initialState = initialState ?? (initialState.EmptyIfNull().ToLookup( x => x.Key, x => x.First() ));

            return ui.For( fieldName, initialState,
                ui.Html.ViewContext.HttpContext.Composition().GetMany<AuditEventKind>()
                .ToDictionary( kind => kind, kind => factories.FirstOrDefault( f => f.AppliesToEventKind( kind ) ) )
            );
        }
    }
}