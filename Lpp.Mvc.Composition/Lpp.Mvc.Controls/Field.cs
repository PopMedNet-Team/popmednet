using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics.Contracts;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using Lpp.Composition;

namespace Lpp.Mvc.Controls
{
    class Field : IField
    {
        [Export, ExportMetadata( ExportScope.Key, TransactionScope.Id )]
        public static IUIWidgetFactory<IField> Factory { get { return UIWidget.Factory<IField>( html => new Field { Html = html } ); } }

        public HtmlHelper Html { get; private set; }

        public IHtmlString Get( string label, Func<HtmlHelper, IHtmlString> content, string cssClass )
        {
            return Html.Partial<Lpp.Mvc.Views.Field>().WithModel(new FieldModel { Label = label, Content = content, CssClass = cssClass });
        }
    }

    public class FieldModel
    {
        public string Label { get; set; }
        public string CssClass { get; set; }
        public Func<HtmlHelper, IHtmlString> Content { get; set; }
    }
}