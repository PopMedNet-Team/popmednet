using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lpp.Mvc;

namespace Lpp.Audit.UI
{
    /// <summary>
    /// This widget produces output (in the 'fieldName' hidden field) in the format:
    ///     Event-Kind-ID "/" Filter-Factory-ID ":" Filter-String
    ///     
    /// where "Filter-String" is the output produced by the corresponding filter UI.
    /// To parse this output, use <see cref="IAuditUI.ParseFilters"/>
    /// </summary>
    public interface IAuditEventFiltersUI : IUIWidget
    {
        IHtmlString For( string fieldName, ILookup<AuditEventKind, IAuditEventFilter> initialState,
            IDictionary<AuditEventKind, IAuditEventFilterUIFactory> defaultFilterUIs );
    }
}