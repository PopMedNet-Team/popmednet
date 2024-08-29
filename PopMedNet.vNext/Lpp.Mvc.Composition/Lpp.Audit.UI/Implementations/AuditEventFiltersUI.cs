using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc;
using Lpp.Composition;

namespace Lpp.Audit.UI
{
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    public class AuditEventFiltersUI : IAuditEventFiltersUI
    {
        [Export] public static IUIWidgetFactory<IAuditEventFiltersUI> Factory { get { return UIWidget.Factory<IAuditEventFiltersUI>( html => new AuditEventFiltersUI { Html = html } ); } }
        public HtmlHelper Html { get; private set; }

        public IHtmlString For( string fieldName, ILookup<AuditEventKind, IAuditEventFilter> initialState, 
            IDictionary<AuditEventKind, IAuditEventFilterUIFactory> defaultFilterUIs )
        {
            var filterUIs = Html.ViewContext.HttpContext.Composition().GetMany<IAuditEventFilterUIFactory>();

            return Html.Partial<Views.AuditEventFiltersUI>().WithModel( new Models.AuditEventFiltersUIModel
            {
                FieldName = fieldName,
                InitialState = (from k in initialState
                                from f in k
                                join ui in filterUIs on f.Factory equals ui.FilterFactory into uis
                                from ui in uis.DefaultIfEmpty()
                                let display = ui == null ? null : new Models.EventKindFiltersModel { Display = ui.RenderFilterUI( k.Key, f ), FactoryId = ui.FilterFactory.Id }
                                select new { kind = k.Key, display }
                               )
                               .ToLookup( x => x.kind, x => x.display ),

                DefaultFilterUIs = defaultFilterUIs.ToDictionary(
                                        x => x.Key,
                                        x => x.Value == null ? null : new Models.EventKindFiltersModel
                                        {
                                            Display = x.Value.RenderFilterUI( x.Key, null ),
                                            FactoryId = x.Value.FilterFactory.Id
                                        } )
            } );
        }
    }
}