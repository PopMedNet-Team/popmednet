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

namespace Lpp.Composition
{
    public static class CompositionExtensions
    {
        public static T Compose<T>( this ICompositionService compositionService, T obj ) where T : class
        {
            compositionService.SatisfyImportsOnce( obj );
            return obj;
        }

        public static object Get( this ICompositionService cs, Type componentType )
        {
            var helperType = typeof( Helper<> ).MakeGenericType( componentType );
            return ( cs.Compose( Activator.CreateInstance( helperType ) ) as IHelper ).Value;
        }

        public static IEnumerable<object> GetMany( this ICompositionService cs, Type componentType )
        {
            var helperType = typeof( ManyHelper<> ).MakeGenericType( componentType );
            return cs.Compose( Activator.CreateInstance( helperType ) ) as IEnumerable<object>;
        }

        public static T Get<T>( this ICompositionService cs ) where T : class
        {
            return cs.Compose( new Helper<T>() ).Value;
        }

        public static IEnumerable<T> GetMany<T>( this ICompositionService cs ) where T : class
        {
            return cs.Compose( new ManyHelper<T>() ).Value;
        }

        interface IHelper { object Value { get; } }

        private class Helper<T> : IHelper
        {
            [Import]
            public T Value { get; set; }

            object IHelper.Value { get { return Value; } }
        }

        private class ManyHelper<T> : IEnumerable<T>
        {
            [ImportMany]
            public IEnumerable<T> Value { get; set; }

            public IEnumerator<T> GetEnumerator()
            {
                return Value.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return Value.GetEnumerator();
            }
        }
    }
}