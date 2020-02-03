using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Security.Principal;
using System.Web;
using System.Reactive.Linq;
using System.Threading;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Web.Mvc;
using System.Collections.Concurrent;
using System.Reflection.Emit;
using System.Reflection;
using System.Linq.Expressions;

namespace Lpp.Mvc
{
    public class PlainUIWidget : IUIWidget, IHtmlString
    {
        public HtmlHelper Html { get; private set; }
        private readonly IHtmlString _value;

        public PlainUIWidget( HtmlHelper html, IHtmlString value )
        {
            //Contract.Requires( html != null );
            //Contract.Requires( value != null );
            Html = html;
            _value = value;
        }

        public string ToHtmlString()
        {
            return _value.ToHtmlString();
        }
    }

    public static class PlainUIWidgetExtensions
    {
        public static TWidgetInterface UIWidgetFromString<TWidgetInterface>( this HtmlHelper html, IHtmlString str )
            where TWidgetInterface : class, IUIWidget, IHtmlString
        {
            //Contract.Requires( html != null );
            //Contract.Ensures( //Contract.Result<TWidgetInterface>() != null );
            return (TWidgetInterface) GetPlainWidgetImplementation( typeof( TWidgetInterface ) )( html, str );
        }

        public static TWidgetInterface UIWidgetFromString<TWidgetInterface>( this HtmlHelper html, string str )
            where TWidgetInterface : class, IUIWidget, IHtmlString
        {
            //Contract.Requires( html != null );
            //Contract.Ensures( //Contract.Result<TWidgetInterface>() != null );
            return html.UIWidgetFromString<TWidgetInterface>( new MvcHtmlString( str ) );
        }

        public static UIWidgetFromPartialBuilder<TWidgetInterface,TPartial> UIWidgetFromPartial<TWidgetInterface, TPartial>( this HtmlHelper html )
            where TWidgetInterface : class, IUIWidget, IHtmlString
            where TPartial : WebViewPage
        {
            //Contract.Requires( html != null );
            return new UIWidgetFromPartialBuilder<TWidgetInterface, TPartial> { Html = html };
        }

        public static TWidgetInterface WithModel<TWidgetInterface, TPartial, TModel>( 
            this UIWidgetFromPartialBuilder<TWidgetInterface, TPartial> wb, TModel model )
            where TWidgetInterface : class, IUIWidget, IHtmlString
            where TPartial : WebViewPage<TModel>
        {
            //Contract.Ensures( //Contract.Result<TWidgetInterface>() != null );
            return wb.Html.UIWidgetFromString<TWidgetInterface>( wb.Html.Partial<TPartial>().WithModel( model ) );
        }

        public static TWidgetInterface WithoutModel<TWidgetInterface, TPartial>( this UIWidgetFromPartialBuilder<TWidgetInterface, TPartial> wb )
            where TWidgetInterface : class, IUIWidget, IHtmlString
            where TPartial : WebViewPage
        {
            //Contract.Ensures( //Contract.Result<TWidgetInterface>() != null );
            return wb.Html.UIWidgetFromString<TWidgetInterface>( wb.Html.Partial<TPartial>().WithoutModel() );
        }

        public class UIWidgetFromPartialBuilder<TWidgetInterface, TPartial> { public HtmlHelper Html { get; set; } }

        private static readonly Lazy<ModuleBuilder> _widgetsModule = new Lazy<ModuleBuilder>( () =>
        {
            var name = "UIWidgets_" + Guid.NewGuid().ToString();
            var asm = AppDomain.CurrentDomain.DefineDynamicAssembly( new AssemblyName( name ), AssemblyBuilderAccess.Run );
            return asm.DefineDynamicModule( name, true );
        }, isThreadSafe: true );

        private static readonly ConcurrentDictionary<string, Func<HtmlHelper, IHtmlString, object>> _widgetFactories = new ConcurrentDictionary<string, Func<HtmlHelper, IHtmlString, object>>();

        private static Func<HtmlHelper, IHtmlString, object> GetPlainWidgetImplementation( Type intf )
        {
            //Contract.Requires( intf != null );
            //Contract.Requires( intf.IsInterface );
            //Contract.Requires( typeof( IHtmlString ).IsAssignableFrom( intf ) );
            //Contract.Requires( typeof( IUIWidget ).IsAssignableFrom( intf ) );

            return _widgetFactories.GetOrAdd( intf.AssemblyQualifiedName, _ =>
            {
                var typeBuilder = _widgetsModule.Value.DefineType( 
                    intf.Name + "_" + Guid.NewGuid().ToString().Replace( "-", "" ) + "_Type", 
                    TypeAttributes.Class, typeof( PlainUIWidget ), new[] { intf } );

                var ctor = typeBuilder
                    .DefineConstructor( MethodAttributes.Public, CallingConventions.Any, new[] { typeof( HtmlHelper ), typeof( IHtmlString ) } );
                var il = ctor.GetILGenerator();
                il.Emit( OpCodes.Ldarg_0 );
                il.Emit( OpCodes.Ldarg_1 );
                il.Emit( OpCodes.Ldarg_2 );
                il.Emit( OpCodes.Call, typeof( PlainUIWidget ).GetConstructors().First() );
                il.Emit( OpCodes.Ret );

                var type = typeBuilder.CreateType();

                var helperParam = Expression.Parameter( typeof( HtmlHelper ) );
                var stringParam = Expression.Parameter( typeof( IHtmlString ) );
                return Expression.Lambda<Func<HtmlHelper, IHtmlString, object>>(
                    Expression.New( type.GetConstructors().First(), helperParam, stringParam ),
                    helperParam, stringParam
                ).Compile();
            } );
        }
    }
}