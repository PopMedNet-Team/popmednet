using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web.WebPages.Razor;
using System.Web.Hosting;
using System.Collections.Concurrent;
using System.Web.WebPages;
using System.Web.Routing;
using System.Web;
using System.Linq.Expressions;
using System.Collections.Specialized;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc
{
    [ModelBinder( typeof( RoutedComputationBinder ) )]
    public class RoutedComputation<TResult> : IRoutedComputation
    {
        private readonly Lazy<TResult> _value;

        public RoutedComputation( RequestContext reqCtx, string route )
        {
            Route = route;
            _value = new Lazy<TResult>( () =>
            {
                var httpCtx = new HttpCtx( HttpContext.Current, route );
                var rd = RouteTable.Routes.GetRouteData( httpCtx );
                if ( rd == null ) throw new InvalidOperationException();

                var reqc = new RequestContext( httpCtx, rd );

                var cf = ControllerBuilder.Current.GetControllerFactory();
                var ctrName = rd.GetRequiredString( "controller" );
                var ctrl = cf.CreateController( reqc, ctrName ) as Controller;
                if ( ctrl == null ) throw new InvalidOperationException( "Cannot find controller '" + ctrName + "' or it does not inherit from System.Web.Mvc.Controller" );

                var res = ResultSink.Push( httpCtx, route );

                try
                {
                    ctrl.ControllerContext = new ControllerContext( reqc, ctrl );
                    ctrl.ValueProvider = new ValueProviderCollection( new IValueProvider[]
                    { 
                        new NameValueCollectionValueProvider( httpCtx.Request.QueryString, System.Threading.Thread.CurrentThread.CurrentCulture ),
                        new DictionaryValueProvider<object>( rd.Values.ToDictionary( v => v.Key, v => v.Value ), System.Threading.Thread.CurrentThread.CurrentCulture )
                    } );
                    if ( false == ctrl.ActionInvoker.InvokeAction( ctrl.ControllerContext, rd.GetRequiredString( "action" ) ) )
                    {
                        throw new InvalidOperationException( string.Format( "Cannot find action '{0}' on controller '{1}' or it did not pass current filter, such as HTTP method, auth requirements, etc.", rd.GetRequiredString( "action" ), ctrName ) );
                    }

                    return res.Result;
                }
                finally
                {
                    cf.ReleaseController( ctrl );
                    res.Pop( httpCtx );
                }
            } );
        }

        public string Route { get; private set; }
        public TResult Compute() { return _value.Value; }
        public override string ToString() { return Route; }

        class HttpCtx : HttpContextWrapper 
        {
            private readonly HttpRequestWrapper _req;
            public override HttpRequestBase Request { get { return _req; } }

            public HttpCtx( HttpContext inner, string url )
                : base( inner )
            {
                var orgUrl = inner.Request.Url;
                var parts = url.Split( new[] { '?' }, 2 );
                var uri = new UriBuilder( orgUrl.Scheme, orgUrl.Host, orgUrl.Port, parts[0], "" ).Uri;
                var req = new HttpRequest( "", uri.AbsoluteUri, parts.ElementAtOrDefault( 1 ) );
                _req = new HttpRequestWrapper( req );
            }
        }

        public class ResultSink
        {
            private object _prev;
            private string _route;

            const string Key = "{C784C417-E467-45FA-B0E8-3CD3E4100C90}";

            public static ResultSink Current( HttpContextBase ctx )
            {
                var r = ctx.Items[Key];
                var res = r as ResultSink;
                if ( res == null ) throw new InvalidOperationException( "Either computation was not expected in this context or a different result type was expected. " + r.GetType().Name );
                return res;
            }

            public static ResultSink Push( HttpContextBase ctx, string route )
            {
                var r = new ResultSink { _prev = ctx.Items[Key], _route = route };
                ctx.Items[Key] = r;
                return r;
            }

            public void Pop( HttpContextBase ctx )
            {
                if ( ctx.Items[Key] != this ) throw new InvalidOperationException();
                ctx.Items[Key] = _prev;
            }

            private bool _resultSet = false;
            private TResult _result;
            public TResult Result { get { if ( _resultSet ) return _result; throw new InvalidOperationException( "Routed computation '" + _route + "' did not produce a result" ); } }
            public void SetResult( TResult res ) { _result = res; _resultSet = true; } // TODO: Protection from setting twice
        }
    }

    public class ComputationResult<T> : ActionResult
    {
        private readonly T _result;
        public ComputationResult( T result ) { _result = result; }

        public override void ExecuteResult( ControllerContext context )
        {
            RoutedComputation<T>.ResultSink.Current( context.HttpContext ).SetResult( _result );
        }
    }

    public static class Computation
    {
        public static ComputationResult<T> Result<T>( T value ) { return new ComputationResult<T>( value ); }
    }

    public interface IRoutedComputation
    {
        string Route { get; }
    }

    class RoutedComputationBinder : IModelBinder
    {
        public object BindModel( ControllerContext controllerContext, ModelBindingContext bindingContext )
        {
            if ( bindingContext.ModelType.IsGenericType && bindingContext.ModelType.GetGenericTypeDefinition() == typeof( RoutedComputation<> ) )
            {
                var res = from v in Maybe.Value( bindingContext.ValueProvider.GetValue( bindingContext.ModelName ) )
                          from s in v.ConvertTo( typeof( string ) ) as string
                          select Activator.CreateInstance( bindingContext.ModelType, controllerContext.RequestContext, s );
                return res.ValueOrNull();
            }

            return null;
        }
    }

    public static class RoutedComputationExtensions
    {
        public static RoutedComputation<T> RoutedComputation<TController, T>( this UrlHelper url, Expression<Func<TController, ComputationResult<T>>> compute )
            where TController : Controller
        {
            //Contract.Requires( url != null );
            //Contract.Requires( compute != null );
            //Contract.Ensures( //Contract.Result<RoutedComputation<T>>() != null );

            return new RoutedComputation<T>( url.RequestContext, url.Action<TController>( compute.Cast().As<object>() ) );
        }
    }
}