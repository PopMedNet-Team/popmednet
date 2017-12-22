using System;
using System.Collections.Generic;
using System.Linq;
using Lpp.Composition;
using Lpp.Dns.Model;
using Lpp.Dns.Tests;
using Lpp.Security;
//using Xunit;

namespace Lpp.Dns.Portal.Tests
{
    //public class GroupServiceTests : TestBase
    //{
    //    [Fact]
    //    public void GroupService_Basic()
    //    {
    //        new[] { "one", "two", "one-1", "one-2", "one-1-1" }.FromNames<Organization>().InsertEntities( this );
    //        new[] { "g1", "g2" }.FromNames<Group>().InsertEntities( this );

    //        DbTran( scope =>
    //        {
    //            var g1 = scope.ByName<Group>( "g1" );
    //            var g2 = scope.ByName<Group>( "g2" );
    //            var h = scope.Get<ISecurityObjectHierarchyService<DnsDomain>>();
    //            Action<string, Group> g = ( o, grp ) => scope.ByName<Organization>( o ).Groups.Add( grp );
    //            Action<string, string> p = ( o, pr ) =>
    //            {
    //                var oo = scope.ByName<Organization>( o );
    //                var pp = scope.ByName<Organization>( pr );
    //                oo.Parent = pp;
    //                h.SetObjectInheritanceParent( oo, pp );
    //            };

    //            foreach ( var o in scope.Entities<Organization>() ) h.SetObjectInheritanceParent( o, VirtualSecObjects.AllOrganizations );
    //            foreach ( var gg in scope.Entities<Group>() ) h.SetObjectInheritanceParent( gg, VirtualSecObjects.AllGroups );

    //            p( "one-1", "one" );
    //            p( "one-2", "one" );
    //            p( "one-1-1", "one-1" );

    //            g( "one", g1 );
    //            g( "two", g2 );
    //            g( "one-1", g2 );
    //        } );

    //        DbTran( scope =>
    //        {
    //            var s = scope.Get<GroupService>();
    //            Func<string,IEnumerable<string>> ms = n => s.GetEffectiveMembersOf( scope.ByName<Group>( n ) ).Select( o => o.Name ).AsEnumerable().OrderBy( x => x );

    //            Assert.Equal( new[] { "one", "one-1", "one-1-1", "one-2" }, ms( "g1" ) );
    //            Assert.Equal( new[] { "one-1", "one-1-1", "two" }, ms( "g2" ) );
    //        } );
    //    }
    //    }
}