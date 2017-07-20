using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using Lpp.Composition;
//using Xunit;
using System.Web;
using System.Data.Entity;
using System.Data.Common;
using Lpp.Tests;
//using Xunit.Extensions;
using System.Data.Entity.Infrastructure;

namespace Lpp.Security.Tests
{
    //public class AccessControl : IUseFixture<DatabaseFixture>, IUseFixture<CompositionFixture>
    //{
    //    DatabaseFixture DbFixture { get; set; }
    //    CompositionFixture Composition { get; set; }

    //    [Fact]
    //    public void Security_Inheritance_Basic()
    //    {
    //        var a = new Obj { Kind = Kind1 };
    //        var b = new Obj { Kind = Kind1 };
    //        var c = new Obj { Kind = Kind1 };
    //        var i = new Obj { Kind = Kind2 };
    //        var j = new Obj { Kind = Kind2 };
    //        var k = new Obj { Kind = Kind2 };
    //        var subj = new Subj { DisplayName = "User" };
    //        var acl = new[] { 
    //            new AclEntry { Subject = subj, Kind = AclEntryKind.Allow, Privilege = Privilege1 },
    //            new AclEntry { Subject = subj, Kind = AclEntryKind.Deny, Privilege = Privilege2 }
    //        };
    //        var localAcl = new[] {
    //            new AclEntry { Subject = subj, Privilege = Privilege1, Kind = AclEntryKind.Deny }
    //        };
    //        var assertCan = new Action<Ctx, SecurityTarget, SecurityPrivilege>( ( x, t, p ) => Assert.True( x.Sec.Can( t, subj )( p ) ) );
    //        var assertCannot = new Action<Ctx, SecurityTarget, SecurityPrivilege>( ( x, t, p ) => Assert.False( x.Sec.Can( t, subj )( p ) ) );
    //        var test = Test( new[] { subj }, new[] { a, b, c, i, j, k } );

    //        CleanUp();
    //        test( x =>
    //        {
    //            x.Hierarchy.SetObjectInheritanceParent( b, a );
    //            x.Hierarchy.SetObjectInheritanceParent( c, a );
    //            x.Hierarchy.SetObjectInheritanceParent( j, i );
    //            x.Hierarchy.SetObjectInheritanceParent( k, i );
    //            x.Uow.Commit();
    //        } );

    //        test( x =>
    //        {
    //            Assert.Equal( Enumerable.Empty<UnresolvedAclEntry>(), x.Sec.GetAcl( Sec.Target( j, b ) ) );
    //            assertCannot( x, Sec.Target( j, a ), Privilege1 );
    //            assertCannot( x, Sec.Target( j, a ), Privilege2 );
    //            assertCannot( x, Sec.Target( j, b ), Privilege1 );
    //            assertCannot( x, Sec.Target( i, a ), Privilege2 );
    //            assertCannot( x, Sec.Target( j, a ), Privilege1 );
    //            assertCannot( x, Sec.Target( k, b ), Privilege2 );

    //            x.Sec.SetAcl( Sec.Target( j, a ), acl );
    //            x.Uow.Commit();
    //        } );

    //        test( x =>
    //        {
    //            Assert.Equal( 
    //                new[] { 
    //                    AclEntry( subj, Privilege1, AclEntryKind.Allow, j, a ), 
    //                    AclEntry( subj, Privilege2, AclEntryKind.Deny, j, a ) 
    //                },
    //                x.Sec.GetAcl( Sec.Target( j, b ) ).ResolveAcl( x.Sec, Target ).OrderBy( e => e.Entry.Privilege == Privilege1 ? 0 : 1 ),
    //                AclEntryComparer.Instance );
    //            new[] { a, b, c }.ForEach( o =>
    //            {
    //                assertCan( x, Sec.Target( j, o ), Privilege1 );
    //                assertCannot( x, Sec.Target( j, o ), Privilege2 );
    //            } );
    //            new[] { i, k }.ForEach( p =>
    //                new[] { a, b, c }.ForEach( o =>
    //                {
    //                    assertCannot( x, Sec.Target( p, o ), Privilege1 );
    //                    assertCannot( x, Sec.Target( p, o ), Privilege2 );
    //                } ) );
    //            Assert.Equal( new[] { a, b, c }.Select( o => Sec.Target( j, o ).Id() ).OrderBy( t => t ),
    //                x.Sec.AllGrantedTargets( subj, Privilege1, 2 ).ToList().OrderBy( t => t ) );
    //            Assert.False( x.Sec.AllGrantedTargets( subj, Privilege2, 2 ).Any() );

    //            x.Sec.SetAcl( Sec.Target( j, b ), localAcl );
    //            x.Uow.Commit();
    //        } );

    //        test( x =>
    //        {
    //            Assert.Equal( new[] { 
    //                    AclEntry( subj, Privilege1, AclEntryKind.Deny ), 
    //                    AclEntry( subj, Privilege2, AclEntryKind.Deny, j, a ) 
    //                },
    //                x.Sec.GetAcl( Sec.Target( j, b ) ).ResolveAcl( x.Sec, Target )
    //                .OrderBy( e => e.Entry.Privilege == Privilege1 ? 0 : 1 ),
    //                AclEntryComparer.Instance );
    //            Assert.Equal( 
    //                new[] { AclEntry( subj, Privilege1, AclEntryKind.Deny ) }, 
    //                x.Sec.GetAcl( Sec.Target( j, b ) ).Where( e => !e.IsInherited ).ResolveAcl( x.Sec, Target ), AclEntryComparer.Instance );
    //            Assert.Equal( new[] { 
    //                    AclEntry( subj, Privilege1, AclEntryKind.Allow, j, a ), 
    //                    AclEntry( subj, Privilege2, AclEntryKind.Deny, j, a ) 
    //                }, 
    //                x.Sec.GetAcl( Sec.Target( j, c ) ).ResolveAcl( x.Sec, Target )
    //                .OrderBy( e => e.Entry.Privilege == Privilege1 ? 0 : 1 ), 
    //                AclEntryComparer.Instance );
    //            Assert.Equal( Enumerable.Empty<UnresolvedAclEntry>(), x.Sec.GetAcl( Sec.Target( j, c ) ).Where( e => !e.IsInherited ) );

    //            new[] { i, k }.ForEach( p =>
    //                {
    //                    assertCannot( x, Sec.Target( p, b ), Privilege1 );
    //                    assertCannot( x, Sec.Target( p, b ), Privilege2 );
    //                } );
    //            new[] { a, c }.ForEach( o =>
    //                {
    //                    assertCan( x, Sec.Target( j, o ), Privilege1 );
    //                    assertCannot( x, Sec.Target( j, o ), Privilege2 );
    //                } );
    //        } );
    //    }

    //    [Fact]
    //    public void Security_NoInheritance()
    //    {
    //        var a = new Obj { Kind = Kind1 };
    //        var b = new Obj { Kind = Kind2 };
    //        var subj = new Subj { DisplayName = "User" };
    //        var test = Test( new[] { subj }, new[] { a, b } );

    //        CleanUp();
    //        test( x =>
    //        {
    //            x.Sec.SetAcl( new SecurityTarget( a, b ), new[] { new AclEntry { Subject = subj, Kind = AclEntryKind.Allow, Privilege = Privilege1 } } );
    //            x.Uow.Commit();
    //        } );

    //        test( x =>
    //        {
    //            Assert.True( x.Sec.HasPrivilege( new SecurityTarget( a, b ), subj, Privilege1 ) );

    //            x.Sec.SetAcl( new SecurityTarget( a, b ), new[] { new AclEntry { Subject = subj, Kind = AclEntryKind.Deny, Privilege = Privilege1 } } );
    //            x.Uow.Commit();
    //        } );

    //        test( x =>
    //        {
    //            Assert.False( x.Sec.HasPrivilege( new SecurityTarget( a, b ), subj, Privilege1 ) );
    //        } );
    //    }

    //    [Theory]
    //    [InlineData( "{FC63824A-2524-449F-BD46-57249952C4F8}", "{AB584B5F-9715-4A7D-8A9A-83D3079589A8}", "{9CAAEF4E-5C07-40CE-A154-BE04FDFB1AEF}" )]
    //    [InlineData( "{FC63824A-2524-449F-BD46-57249952C4F8}", "{9CAAEF4E-5C07-40CE-A154-BE04FDFB1AEF}", "{AB584B5F-9715-4A7D-8A9A-83D3079589A8}" )]
    //    [InlineData( "{AB584B5F-9715-4A7D-8A9A-83D3079589A8}", "{FC63824A-2524-449F-BD46-57249952C4F8}", "{9CAAEF4E-5C07-40CE-A154-BE04FDFB1AEF}" )]
    //    [InlineData( "{AB584B5F-9715-4A7D-8A9A-83D3079589A8}", "{9CAAEF4E-5C07-40CE-A154-BE04FDFB1AEF}", "{FC63824A-2524-449F-BD46-57249952C4F8}" )]
    //    [InlineData( "{9CAAEF4E-5C07-40CE-A154-BE04FDFB1AEF}", "{AB584B5F-9715-4A7D-8A9A-83D3079589A8}", "{FC63824A-2524-449F-BD46-57249952C4F8}" )]
    //    [InlineData( "{9CAAEF4E-5C07-40CE-A154-BE04FDFB1AEF}", "{FC63824A-2524-449F-BD46-57249952C4F8}", "{AB584B5F-9715-4A7D-8A9A-83D3079589A8}" )]
    //    public void Security_Membership_Basic( string id1, string id2, string id3 )
    //    {
    //        var obj1 = new Obj { Kind = Kind1 };
    //        var obj2 = new Obj { Kind = Kind2 };
    //        var target = Sec.Target( obj1, obj2 );
    //        var group1 = new Subj { SID = new Guid( id1 ), DisplayName = "Group1" };
    //        var group2 = new Subj { SID = new Guid( id2 ), DisplayName = "Group2" };
    //        var subj = new Subj { SID = new Guid( id3 ), DisplayName = "Subj" };

    //        CleanUp();
    //        var test = Test( new[] { subj, group1, group2 }, new[] { obj1, obj2 } );
    //        var assert = new Action<Ctx, ISecuritySubject, SecurityPrivilege, bool>( ( x, s, p, can ) =>
    //        {
    //            Assert.Equal( can, x.Sec.Can( target, s )( p ) );
    //            Assert.Equal( can ? new[] { target.Id() } : Enumerable.Empty<BigTuple<Guid>>(), x.Sec.AllGrantedTargets( s, p, 2 ) );
    //        } );
    //        var assertCan = new Action<Ctx, ISecuritySubject, SecurityPrivilege>( ( x, s, p ) => assert( x, s, p, true ) );
    //        var assertCannot = new Action<Ctx, ISecuritySubject, SecurityPrivilege>( ( x, s, p ) => assert( x, s, p, false ) );

    //        var acl = new[] { new AclEntry { Subject = group1, Privilege = Privilege1, Kind = AclEntryKind.Allow } }.ToList();

    //        test( x =>
    //        {
    //            x.Sec.SetAcl( target, acl );
    //            x.Membership.SetSubjectParents( group2, new[] { group1 } );
    //            x.Membership.SetSubjectParents( subj, new[] { group2 } );
    //            x.Uow.Commit();
    //        } );

    //        test( x =>
    //        {
    //            new[] { subj, group1, group2 }.ForEach( s =>
    //            {
    //                assertCan( x, s, Privilege1 );
    //                assertCannot( x, s, Privilege2 );
    //            } );

    //            acl.Add( new AclEntry { Subject = group2, Privilege = Privilege1, Kind = AclEntryKind.Deny } );
    //            acl.Add( new AclEntry { Subject = group2, Privilege = Privilege2, Kind = AclEntryKind.Allow } );
    //            x.Sec.SetAcl( target, acl );
    //            x.Uow.Commit();
    //        } );

    //        test( x =>
    //        {
    //            new[] { subj, group2 }.ForEach( s =>
    //            {
    //                assertCan( x, s, Privilege2 );
    //                assertCannot( x, s, Privilege1 );
    //            } );
    //            assertCannot( x, group1, Privilege2 );
    //            assertCan( x, group1, Privilege1 );

    //            acl.Add( new AclEntry { Subject = subj, Privilege = Privilege1, Kind = AclEntryKind.Allow } );
    //            x.Sec.SetAcl( target, acl );
    //            x.Uow.Commit();
    //        } );

    //        test( x =>
    //        {
    //            new[] { subj, group2 }.ForEach( s =>
    //            {
    //                assertCan( x, s, Privilege2 );
    //                assertCannot( x, s, Privilege1 );
    //            } );
    //            assertCannot( x, group1, Privilege2 );
    //            assertCan( x, group1, Privilege1 );

    //            acl.RemoveAll( e => e.Privilege == Privilege1 && e.Subject == group2 && e.Kind == AclEntryKind.Deny );
    //            x.Sec.SetAcl( target, acl );
    //            x.Uow.Commit();
    //        } );

    //        test( x =>
    //        {
    //            new[] { group2, subj }.ForEach( s =>
    //            {
    //                assertCan( x, s, Privilege2 );
    //                assertCan( x, s, Privilege1 );
    //            } );
    //            assertCannot( x, group1, Privilege2 );
    //            assertCan( x, group1, Privilege1 );
    //        } );
    //    }

    //    [Fact]
    //    public void Security_Avoid_Duplicate_Entries()
    //    {
    //        var obj = new Obj { Kind = Kind1 };
    //        var target = Sec.Target( obj );
    //        var subj = new Subj { SID = Guid.NewGuid(), DisplayName = "Subj" };

    //        CleanUp();
    //        var test = Test( new[] { subj }, new[] { obj } );

    //        test( x =>
    //        {
    //            x.Sec.SetAcl( target, new[] { 
    //                new AclEntry { Kind = AclEntryKind.Allow, Subject = subj, Privilege = Privilege1 },
    //                new AclEntry { Kind = AclEntryKind.Allow, Subject = subj, Privilege = Privilege2 }
    //            });
    //            x.Uow.Commit();

    //            using ( var db = new Db( x.ConnectionString ) )
    //            {
    //                db.Database.ExecuteSqlCommand( 
    //                    "insert into AclEntries( TargetId, SubjectId, PrivilegeId, [Order], Allow ) select Id, {0}, {1}, 0, 1 from SecurityTargets",
    //                    subj.SID, Privilege2.SID );
    //                db.SaveChanges();
    //            }
    //        } );

    //        test( x =>
    //        {
    //            x.Sec.SetAcl( target, new[] { 
    //                new AclEntry { Kind = AclEntryKind.Allow, Subject = subj, Privilege = Privilege2 }
    //            });
    //            x.Uow.Commit();
    //        } );
    //    }

    //    static AnnotatedAclEntry AclEntry( ISecuritySubject subj, SecurityPrivilege p, AclEntryKind kind, params ISecurityObject[] inheritedFrom )
    //    {
    //        return new AnnotatedAclEntry
    //        {
    //            Entry = new AclEntry
    //            {
    //                Subject = subj,
    //                Privilege = p,
    //                Kind = kind
    //            },
    //            InheritedFrom = inheritedFrom.Length == 0 ? null : new SecurityTarget( inheritedFrom )
    //        };
    //    }

    //    void CleanUp()
    //    {
    //        using ( var db = new Db( DbFixture.ConnectionString ) )
    //        {
    //            RemoveAll<Data.InheritanceEdge>( db );
    //            RemoveAll<Data.SecurityTarget>( db );
    //            RemoveAll<Data.AclEntry>( db );
    //            RemoveAll<Data.MembershipEdge>( db );
    //            db.SaveChanges();
    //        }
    //    }

    //    Action<Action<Ctx>> Test( IEnumerable<ISecuritySubject> subjs, IEnumerable<ISecurityObject> objs )
    //    {
    //        return t =>
    //        {
    //            var scb = Composition.MockScope()
    //                        .Override( new PersistenceConfig<TestDomain> { ConnectionString = DbFixture.ConnectionString } )
    //                        .Module( Sec.Module<TestDomain>() )
    //                        .Override( Target )
    //                        .Override( Privilege1 )
    //                        .Override( Privilege2 )
    //                        .Override<ISecuritySubjectProvider<TestDomain>>( new SubjectProvider( subjs ) );
    //            foreach ( var o in objs )
    //                scb.Override<ISecurityObjectProvider<TestDomain>>( new ExplicitSecObjectProvider<TestDomain>( o ) );
                    
    //            using ( var scope = scb.Build() )
    //            {
    //                var uow = scope.Get<IUnitOfWork<TestDomain>>();
    //                (uow as IObjectContextAdapter).ObjectContext.CommandTimeout = 300;

    //                t( new Ctx
    //                {
    //                    Uow = uow,
    //                    ConnectionString = DbFixture.ConnectionString,
    //                    Hierarchy = scope.Get<ISecurityObjectHierarchyService<TestDomain>>(),
    //                    Membership = scope.Get<ISecurityMembershipService<TestDomain>>(),
    //                    Sec = scope.Get<ISecurityService<TestDomain>>()
    //                } );
    //            }
    //        };
    //    }

    //    private void RemoveAll<T>( Db db ) where T : class
    //    {
    //        var objs = db.Set<T>();
    //        objs.ToList().Select( objs.Remove ).LastOrDefault();
    //    }

    //    class Ctx
    //    {
    //        public string ConnectionString { get; set; }
    //        public IUnitOfWork<TestDomain> Uow { get; set; }
    //        public ISecurityObjectHierarchyService<TestDomain> Hierarchy { get; set; }
    //        public ISecurityMembershipService<TestDomain> Membership { get; set; }
    //        public ISecurityService<TestDomain> Sec { get; set; }
    //    }

    //    class Db : DbContext
    //    {
    //        public Db( string connStr ) : base( connStr ) { }
    //        protected override void OnModelCreating( DbModelBuilder modelBuilder )
    //        {
    //            new Data.SecurityPersistenceDefinition<TestDomain>().BuildModel( modelBuilder );
    //        }
    //    }

    //    static SecurityObjectKind Kind1 = Sec.ObjectKind( "Kind1" );
    //    static SecurityObjectKind Kind2 = Sec.ObjectKind( "Kind2" );
    //    static SecurityPrivilege Privilege1, Privilege2;
    //    static SecurityPrivilegeSet PrivilegeSet = new SecurityPrivilegeSet( "PrivilegeSet" );
    //    static SecurityTargetKind Target;

    //    static AccessControl()
    //    {
    //        Target = Sec.TargetKind( Kind2, Kind1 ).WithPrivilegeSets( PrivilegeSet );
    //        Privilege1 = Sec.Privilege( "{9360EFD6-D1E2-4D8F-8121-0AAC2E00101C}", "Privilege1", PrivilegeSet );
    //        Privilege2 = Sec.Privilege( "{D8B84F44-15B8-44EA-B045-171E98B286D8}", "Privilege2", PrivilegeSet );
    //    }

    //    class Obj : ISecurityObject
    //    {
    //        public Guid SID { get; set; }
    //        public SecurityObjectKind Kind { get; set; }
    //        public Obj() { SID = Guid.NewGuid(); }
    //    }

    //    class Subj : ISecuritySubject
    //    {
    //        public Guid SID { get; set; }
    //        public string  DisplayName { get; set; }
    //        public override string ToString() { return DisplayName; }
    //        public Subj() { SID = Guid.NewGuid(); }
    //    }

    //    class AclEntryComparer : IEqualityComparer<AnnotatedAclEntry>
    //    {
    //        public bool Equals( AnnotatedAclEntry x, AnnotatedAclEntry y )
    //        {
    //            return
    //                x == null ? y == null :
    //                y == null ? false :
    //                (x.Entry.Subject == y.Entry.Subject && x.Entry.Privilege == y.Entry.Privilege && 
    //                 x.Entry.Kind == y.Entry.Kind && 
    //                 ( x.InheritedFrom == null ? y.InheritedFrom == null :
    //                   y.InheritedFrom == null ? false :
    //                   x.InheritedFrom.Elements.SequenceEqual( y.InheritedFrom.Elements )
    //                 ) 
    //                );
    //        }

    //        public int GetHashCode( AnnotatedAclEntry obj )
    //        {
    //            return 0;
    //        }

    //        public static readonly AclEntryComparer Instance = new AclEntryComparer();
    //    }

    //    class SubjectProvider : ISecuritySubjectProvider<TestDomain>
    //    {
    //        readonly IEnumerable<ISecuritySubject> _subjects;
    //        public SubjectProvider( IEnumerable<ISecuritySubject> subjs )
    //        {
    //            _subjects = subjs;
    //        }

    //        public ISecuritySubject Find( Guid id )
    //        {
    //            return _subjects.FirstOrDefault( s => s.SID == id );
    //        }
    //    }

    //    class TestDomain { }

    //    public void SetFixture( DatabaseFixture data ) { DbFixture = data; }
    //    public void SetFixture( CompositionFixture data ) { Composition = data; }
    //}
}