using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Composition.Modules;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Lpp.Utilities.Legacy;

namespace Lpp.Audit.Data
{
    public class AuditPropertyValue
    {
        public int Id { get; set; }
        public AuditEvent Event { get; set; }
        public Guid PropertyId { get; set; }

        public int IntValue { get; set; }
        public string StringValue { get; set; }
        public double DoubleValue { get; set; }
        public Guid GuidValue { get; set; }
        public DateTime DateTimeValue { get; set; }

        public AuditPropertyValue()
        {
            DateTimeValue = DateTime.Now;
        }

        public static Expression<Func<AuditPropertyValue, TProp>> GetAccessorExpression<TProp>()
        {
            var res = _accessors.ValueOrDefault( typeof( TProp ) );
            return res == null ? null : res.Item1 as Expression<Func<AuditPropertyValue, TProp>>;
        }

        public TProp GetValue<TProp>()
        {
            var res = _accessors.ValueOrDefault( typeof( TProp ) );
            return res == null ? default( TProp ) : (res.Item2 as Func<AuditPropertyValue, TProp>)( this );
        }

        public void SetValue<TProp>( TProp v )
        {
            var res = _setters.ValueOrDefault( typeof( TProp ) );
            if ( res != null ) (res as Action<AuditPropertyValue, TProp>)( this, v );
        }

        static readonly IDictionary<Type, Tuple<LambdaExpression, Delegate>> _accessors = new[]
        {
            E( v => v.IntValue ),
            E( v => v.IntValue != 0 ),
            E( v => v.StringValue ),
            E( v => v.DoubleValue ),
            E( v => (float) v.DoubleValue ),
            E( v => v.GuidValue ),
            E( v => v.DateTimeValue )
        }
        .ToDictionary( x => x.Item1, x => Tuple.Create( x.Item2, x.Item2.Compile() ) );

        static readonly IDictionary<Type, Delegate> _setters = new[]
        {
            A<int>( (v,x) => v.IntValue = x ),
            A<bool>( (v,x) => v.IntValue = x ? 1 : 0 ),
            A<string>( (v,x) => v.StringValue = x ),
            A<double>( (v,x) => v.DoubleValue = x ),
            A<float>( (v,x) => v.DoubleValue = x ),
            A<Guid>( (v,x) => v.GuidValue = x ),
            A<DateTime>( (v,x) => v.DateTimeValue = x )
        }
        .ToDictionary( x => x.Item1, x => x.Item2 );

        static Tuple<Type, LambdaExpression> E<T>( Expression<Func<AuditPropertyValue, T>> expr ) { return Tuple.Create( typeof( T ), (LambdaExpression) expr ); }
        static Tuple<Type, Delegate> A<T>( Action<AuditPropertyValue, T> ac ) { return Tuple.Create( typeof( T ), (Delegate)ac ); }
    }
}