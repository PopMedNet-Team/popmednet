using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Lpp.Security
{
    public abstract class DynamicTuple<TElement, TTuple> : IEquatable<TTuple>
        where TElement : class
        where TTuple : DynamicTuple<TElement, TTuple>
    {
        public IEnumerable<TElement> Elements { get; set; }

        public DynamicTuple() { }
        public DynamicTuple( IEnumerable<TElement> objs ) 
        {
            //Contract.Requires<ArgumentNullException>( objs != null );
            Elements = objs;
        }

        public override bool Equals( object obj )
        {
            return Equals( obj as TTuple );
        }

        public bool Equals( TTuple t )
        {
            if ( t == null ) return false;
            return Elements.SequenceEqual( t.Elements );
        }

        public override int GetHashCode()
        {
            return Elements.Aggregate( 0, ( code, e ) => code + e.GetHashCode() );
        }
    }
}