using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel.Composition;
using Lpp.Utilities.Legacy;

namespace Lpp.Security
{
    public class ExplicitSecObjectProvider<TDomain> : ISecurityObjectProvider<TDomain>
    {
        readonly SortedList<Guid,ISecurityObject> _objects;
        public ISecurityObject Find( Guid id ) { return _objects.ValueOrDefault( id ); }
        public SecurityObjectKind Kind { get; private set; }

        public ExplicitSecObjectProvider( params ISecurityObject[] objs ) : this( objs.AsEnumerable() ) { }
        public ExplicitSecObjectProvider( IEnumerable<ISecurityObject> objs )
        {
            //Contract.Requires<InvalidOperationException>( objs.GroupBy( o => o.Kind ).Count() <= 1, "The initialization list contains objects of more than one kind" );
            _objects = new SortedList<Guid,ISecurityObject>( objs.ToDictionary( o => o.ID ) );
            Kind = objs.MaybeFirst().Select( o => o.Kind ).ValueOrNull();
        }
    }
}