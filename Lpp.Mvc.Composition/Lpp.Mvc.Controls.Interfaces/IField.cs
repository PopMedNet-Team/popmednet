using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics.Contracts;
using Lpp.Mvc;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc.Controls
{
    public interface IField : IUIWidget
    {
        IHtmlString Get( string label, Func<HtmlHelper, IHtmlString> content, string cssClass );
    }

    public static class FieldExtensions
    {
        public static IHtmlString Field( this HtmlHelper html, string label, Func<object, IHtmlString> content, string cssClass = null )
        {
            return html.Render<IField>().Get( label, content, cssClass ?? label );
        }

        public static IHtmlString Field( this HtmlHelper html, string label, IHtmlString content, string cssClass = null )
        {
            return html.Field( label, _ => content, cssClass );
        }

        public static IHtmlString Field( this HtmlHelper html, string label, object value, string cssClass = null )
        {
            return html.Field( label, new MvcHtmlString( Convert.ToString( value ) ), cssClass );
        }

        public static FieldsGenerator<TModel> Fields<TModel>( this HtmlHelper<TModel> html )
        {
            //Contract.Requires( html != null );
            return new FieldsGenerator<TModel>( html, html.ViewData.Model );
        }

        public static FieldsGenerator<TModel> Fields<TModel>( this HtmlHelper html, TModel model )
        {
            //Contract.Requires( html != null );
            return new FieldsGenerator<TModel>( html, model );
        }
    }

    public class FieldsGenerator<TModel> : IHtmlString
    {
        private readonly HtmlHelper _html;
        public HtmlHelper Html { get { return _html; } }

        private readonly TModel _model;
        private readonly FieldsGenerator<TModel> _previous;
        private readonly Func<IHtmlString> _content;
        public FieldsGenerator( HtmlHelper html, TModel model, FieldsGenerator<TModel> prev = null, Func<IHtmlString> content = null )
        { _html = html; _previous = prev; _content = content; _model = model; }

        public FieldsGenerator<TModel> Field( Func<TModel, IHtmlString> content, string label, string cssClass = null )
        {
            return Next( () => _html.Field( label, content( _model ), cssClass ) );
        }
        public FieldsGenerator<TModel> Show( object value, string label, string cssClass = null )
        {
            return Next( () => _html.Field( label, new MvcHtmlString( "<div class=\"Value\">" + value + "</div>" ), cssClass ) );
        }
        public FieldsGenerator<TModel> Show( Expression<Func<TModel, object>> accessor, string label = null, string cssClass = null )
        {
            var member = accessor.MemberName();
            return Show( accessor.Compile()( _model ), label ?? member, cssClass ?? member );
        }

        public FieldsGenerator<TModel> TextBox( Expression<Func<TModel, object>> accessor, string label = null, string cssClass = null )
        {
            return HtmlHelper( accessor, _html.TextBox, label, cssClass );
        }
        public FieldsGenerator<TModel> TextArea( Expression<Func<TModel, object>> accessor, string label = null, string cssClass = null )
        {
            return HtmlHelper( accessor, _html.TextArea, label, cssClass );
        }
        public FieldsGenerator<TModel> DropDown( Expression<Func<TModel, object>> accessor, IEnumerable<SelectListItem> items,
            string label = null, string cssClass = null )
        {
            return HtmlHelper( accessor, (name, value) => 
                {
                    var sv = Convert.ToString( value );
                    return _html.DropDownList( name, items.Select( i => new SelectListItem { Value = i.Value, Text = i.Text, Selected = i.Value == sv } ) );
                }, label, cssClass );
        }

        FieldsGenerator<TModel> HtmlHelper( Expression<Func<TModel, object>> accessor, Func<string,string,IHtmlString> helper, string label, string cssClass )
        {
            var member = accessor.MemberName();
            return Next( () => _html.Field( label ?? member, helper( member, Convert.ToString( accessor.Compile()( _model ) ) ), cssClass ?? member ) );
        }

        FieldsGenerator<TModel> Next( Func<IHtmlString> str ) { return new FieldsGenerator<TModel>( _html, _model, this, str ); }

        public string ToHtmlString()
        {
            return string.Join( "",
                EnumerableEx
                .Generate( this, g => g != null, g => g._previous, g => g._content )
                .Reverse()
                .Where( c => c != null )
                .Select( c => c() )
                .Where( s => s != null )
                .Select( s => s.ToHtmlString() )
            );
        }
    }
}