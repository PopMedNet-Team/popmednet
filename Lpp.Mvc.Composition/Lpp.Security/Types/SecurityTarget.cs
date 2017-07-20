using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Lpp.Security
{
    #pragma warning disable 0660, 0661
    public class SecurityTarget : DynamicTuple<ISecurityObject, SecurityTarget> 
    {
        public SecurityTarget() { }
        public SecurityTarget( IEnumerable<ISecurityObject> objs ) : base( objs ) { }
        public SecurityTarget( params ISecurityObject[] objs ) : base( objs ) { }
        public static bool operator==( SecurityTarget a, SecurityTarget b ) { return ReferenceEquals( a, null ) ? ReferenceEquals( b, null ) : a.Equals( b ); }
        public static bool operator!=( SecurityTarget a, SecurityTarget b ) { return ReferenceEquals( a, null ) ? !ReferenceEquals( b, null ) : !a.Equals( b ); }
    }

    public class SecurityTarget<T1> : SecurityTarget where T1 : ISecurityObject
    { public SecurityTarget( T1 obj ) : base( obj ) { } }

    public class SecurityTarget<T1, T2> : SecurityTarget where T1 : ISecurityObject where T2 : ISecurityObject
    { public SecurityTarget( T1 obj1, T2 obj2 ) : base( obj1, obj2 ) { } }

    public class SecurityTarget<T1, T2, T3> : SecurityTarget
        where T1 : ISecurityObject
        where T2 : ISecurityObject
        where T3 : ISecurityObject
    { public SecurityTarget( T1 obj1, T2 obj2, T3 obj3 ) : base( obj1, obj2, obj3 ) { } }
}
