using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Web.Mvc;
using System.Diagnostics.Contracts;

namespace Lpp.Mvc
{
    public interface IModelContext
    {
        IValueProvider Values { get; }
        T GetModel<T>();
        void AddError( string error );
        bool AnyErrors { get; }
    }

    public class MvcModelContext : IModelContext
    {
        private readonly IValueProvider _values;
        private readonly ModelBinderDictionary _binders;
        private readonly ControllerContext _context;
        private readonly ModelStateDictionary _modelState;

        public IValueProvider Values { get { return _values; } }
        public bool AnyErrors { get { return !_modelState.IsValid; } }

        public MvcModelContext( IValueProvider values, ModelBinderDictionary binders, ControllerContext ctx, ModelStateDictionary modelState )
        {
            //Contract.Requires( values != null );
            //Contract.Requires( binders != null );
            //Contract.Requires( ctx != null );
            //Contract.Requires( modelState != null );

            _values = values;
            _binders = binders;
            _context = ctx;
            _modelState = modelState;
        }

        public T GetModel<T>()
        {
            var binder = _binders.GetBinder( typeof( T ) );
            var res = binder.BindModel( _context, new ModelBindingContext
            {
                FallbackToEmptyPrefix = true,
                ModelMetadata = new ModelMetadata( new EmptyModelMetadataProvider(), null, () => default(T), typeof( T ), "" ),
                ValueProvider = _values
            } );

            return (res is T) ? (T)res : default( T );
        }

        public void AddError( string error )
        {
            _modelState.AddModelError( "", error );
        }
    }
}