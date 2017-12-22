using System;
using System.Diagnostics.Contracts;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Mvc
{
    [ContractClass( typeof( Contracts.IJsControlledViewContract ) )]
    public interface IJsControlledView
    {
        IHtmlString Render( HtmlHelper html, string handle );
    }

    public static class JsControlledView
    {
        public static IJsControlledView Create( Func<HtmlHelper, string, IHtmlString> render )
        {
            //Contract.Requires( render != null );
            //Contract.Ensures( //Contract.Result<IJsControlledView>() != null );
            return new Impl( render );
        }

        class Impl : IJsControlledView
        {
            private readonly Func<HtmlHelper, string, IHtmlString> _render;
            public Impl(Func<HtmlHelper, string, IHtmlString> render)
            {
                //Contract.Requires( render != null );
                _render = render;
            }

            public IHtmlString Render( HtmlHelper html, string handle ) { return _render( html, handle ); }
        }
    }

    namespace Contracts
    {
        [ContractClassFor( typeof( IJsControlledView ) )]
        abstract class IJsControlledViewContract : IJsControlledView
        {
            public IHtmlString Render( HtmlHelper html, string handle )
            {
                //Contract.Requires( html != null );
                //Contract.Requires( !string.IsNullOrEmpty( handle ) );
                //Contract.Ensures( //Contract.Result<IHtmlString>() != null );
                throw new System.NotImplementedException();
            }
        }
    }
}