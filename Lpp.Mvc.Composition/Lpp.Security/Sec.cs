using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Composition.Modules;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
//using Lpp.Data.Composition;
using System.Diagnostics.Contracts;

namespace Lpp.Security
{
    public static class Sec
    {
        public static readonly Guid NullObject = new Guid( "{25AA0BBE-1D06-4D57-8EE9-03449E6CD1AB}" );

        public static IModule Module<TDomain>()
        {
            return new ModuleBuilder()
                //.Export<IPersistenceDefinition<TDomain>, Data.SecurityPersistenceDefinition<TDomain>>()
                //.Export<IPersistenceDefinition<TDomain>, Data.Tuples.Tuples.Persistence<TDomain>>()
                .Export<ISecurityService<TDomain>, SecurityService<TDomain>>()
                .Export<ISecurityObjectHierarchyService<TDomain>, SecurityObjectHierarchyService<TDomain>>()
                .Export<ISecurityMembershipService<TDomain>, SecurityMembershipService<TDomain>>()
                .Export<DagService<TDomain, Guid, Data.MembershipEdge, Data.MembershipClosureEdge>>()
                .Export<DagService<TDomain, Guid, Data.InheritanceEdge, Data.InheritanceClosureEdge>>()
                .CreateModule();
        }

        public static SecurityTarget<T1> Target<T1>( T1 obj ) 
            where T1 : ISecurityObject
        { return new SecurityTarget<T1>( obj ); }

        public static SecurityTarget<T1, T2> Target<T1, T2>( T1 obj1, T2 obj2 )
            where T1 : ISecurityObject
            where T2 : ISecurityObject
        { return new SecurityTarget<T1, T2>( obj1, obj2 ); }

        public static SecurityTarget<T1,T2,T3> Target<T1,T2,T3>( T1 obj1, T2 obj2, T3 obj3 )
            where T1 : ISecurityObject
            where T2 : ISecurityObject
            where T3 : ISecurityObject
        { return new SecurityTarget<T1, T2, T3>( obj1, obj2, obj3 ); }

        public static SecurityTarget Target( params ISecurityObject[] objs ){ 
            return new SecurityTarget( objs ); 
        }

        public static SecurityObjectKind ObjectKind( string name )
        {
            return new SecurityObjectKind { Name = name };
        }

        public static SecurityPrivilege Privilege( string sid, string name, params SecurityPrivilegeSet[] appliesToTargetKinds )
        {
            return Privilege( new Guid( sid ), name, appliesToTargetKinds ); 
        }

        public static SecurityPrivilege Privilege( Guid sid, string name, IEnumerable<SecurityPrivilegeSet> belongsToSets )
        {
            return new SecurityPrivilege( sid, name, belongsToSets );
        }

        public static SecurityTargetKindBuilder TargetKind( params SecurityObjectKind[] objKinds )
        {
            return new SecurityTargetKindBuilder( objKinds );
        }

        public static SecurityTargetKindBuilder TargetKind( IEnumerable<SecurityObjectKind> objKinds )
        {
            return new SecurityTargetKindBuilder( objKinds );
        }

        public struct SecurityTargetKindBuilder
        {
            readonly IEnumerable<SecurityObjectKind> _objKinds;
            public SecurityTargetKindBuilder( IEnumerable<SecurityObjectKind> objKinds ) { _objKinds = objKinds; }
            public SecurityTargetKind WithPrivilegeSets( params SecurityPrivilegeSet[] sets )
            {
                return new SecurityTargetKind( _objKinds, sets );
            }
            public SecurityTargetKind WithPrivilegeSets( IEnumerable<SecurityPrivilegeSet> sets )
            {
                return new SecurityTargetKind( _objKinds, sets );
            }
        }

        public static ISecurityObject MockObject( Guid sid, SecurityObjectKind kind = null ) 
        { 
            return new ObjectImpl { ID = sid, Kind = kind }; 
        }

        class ObjectImpl : ISecurityObject 
        { 
            public Guid ID { get; set; } 
            public SecurityObjectKind Kind { get; set; } 
        }

        public static ISecuritySubject MockSubject( Guid sid, string displayName = null ) {
            return new SubjectImpl { ID = sid, DisplayName = displayName };
        }

        class SubjectImpl : ISecuritySubject 
        { 
            public Guid ID { get; set; } 
            public string DisplayName { get; set; } 
        }
    }
}