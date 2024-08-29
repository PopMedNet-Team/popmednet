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
using System.Diagnostics.Contracts;
//using Xunit.Extensions;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;

namespace Lpp.Security.Tests
{
    //public class SecurityDag : IUseFixture<DatabaseFixture>
    //{
    //    DatabaseFixture DbFixture { get; set; }

    //    [Theory]
    //    [InlineData( "SecurityMembership", null )]
    //    [InlineData( "SecurityInheritance", null )]
    //    [InlineData( "SecurityInheritance", 2 )]
    //    [InlineData( "SecurityInheritance", 3 )]
    //    [InlineData( "SecurityInheritance", 4 )]
    //    [InlineData( "SecurityInheritance", 5 )]
    //    public void Security_DAG_Basic( string table, int? closureIndex )
    //    {
    //        var g1 = g( 1 );
    //        var g2 = g( 2 );
    //        var g3 = g( 3 );
    //        var g4 = g( 4 );
    //        var t = Test( table, closureIndex.ToString() );
    //        var check = CompareClosures( t );
    //        var add = Add( t );
    //        var remove = Remove( t );
    //        t( clear );

    //        add( ce( g1, g2 ) );
    //        check( ce( g1, g2, 1 ) );

    //        add( ce( g2, g3 ) );
    //        check( ce( g1, g2, 1 ), ce( g2, g3, 1 ), ce( g1, g3, 2 ) );

    //        add( ce( g1, g3 ) );
    //        check( ce( g1, g2, 1 ), ce( g2, g3, 1 ), ce( g1, g3, 1 ) );

    //        add( ce( g3, g4 ) );
    //        check( ce( g1, g2, 1 ), ce( g2, g3, 1 ), ce( g1, g3, 1 ), ce( g2, g4, 2 ), ce( g3, g4, 1 ), ce( g1, g4, 2 ) );

    //        remove( ce( g1, g3 ) );
    //        check( ce( g1, g2, 1 ), ce( g2, g3, 1 ), ce( g1, g3, 2 ), ce( g2, g4, 2 ), ce( g3, g4, 1 ), ce( g1, g4, 3 ) );

    //        remove( ce( g1, g2 ) );
    //        check( ce( g2, g3, 1 ), ce( g3, g4, 1 ), ce( g2, g4, 2 ) );

    //        remove( ce( g2, g3 ) );
    //        check( ce( g3, g4, 1 ) );

    //        remove( ce( g3, g4 ) );
    //        check();
    //    }

    //    [Fact]
    //    public void Security_DAG_EmptyLoop()
    //    {
    //        var g1 = g( 1 );
    //        var t = Test();
    //        var check = CompareClosures( t );
    //        var add = Add( t );
    //        t( clear );

    //        add( ce( g1, g1 ) );
    //        check( ce( g1, g1, 0 ) );
    //    }

    //    [Fact]
    //    public void Security_DAG_AlternateRouteCases()
    //    {
    //        Guid g1 = g( 1 ), g2 = g( 2 ), g3 = g( 3 ), g4 = g( 4 ), g5 = g( 5 );
    //        var t = Test();
    //        var check = CompareClosures( t );
    //        var add = Add( t );
    //        t( clear );

    //        add( ce( g2, g3 ) );
    //        check( ce( g2, g3, 1 ) );

    //        add( ce( g1, g2 ) );
    //        check( ce( g2, g3, 1 ), ce( g1, g2, 1 ), ce( g1, g3, 2 ) );

    //        add( ce( g4, g5 ) );
    //        check( ce( g2, g3, 1 ), ce( g1, g2, 1 ), ce( g1, g3, 2 ), ce( g4, g5, 1 ) );

    //        add( ce( g3, g4 ) );
    //        check( ce( g2, g3, 1 ), ce( g1, g2, 1 ), ce( g1, g3, 2 ), ce( g4, g5, 1 ), 
    //            ce( g3, g4, 1 ), ce( g2, g4, 2 ), ce( g1, g4, 3 ),
    //            ce( g3, g5, 2 ), ce( g2, g5, 3 ), ce( g1, g5, 4 ) );
    //    }


    //    [Fact]
    //    public void Security_DAG_AlternateRouteMiddleSegment()
    //    {
    //        Guid g1 = g( 1 ), g2 = g( 2 ), g3 = g( 3 ), g4 = g( 4 );
    //        var t = Test();
    //        var check = CompareClosures( t );
    //        var add = Add( t );
    //        t( clear );

    //        add( ce( g1, g2 ), ce( g3, g4 ) );
    //        check( ce( g1, g2, 1 ), ce( g3, g4, 1 ) );

    //        add( ce( g2, g3 ) );
    //        check( 
    //            ce( g1, g2, 1 ), ce( g1, g3, 2 ), ce( g1, g4, 3 ),
    //            ce( g2, g3, 1 ), ce( g2, g4, 2 ),
    //            ce( g3, g4, 1 ) );
    //    }
			
    //    [Fact]
    //    public void Security_DAG_MultiplePaths_IssuePMN667()
    //    {
    //        var g1 = g( 1 );
    //        var g2 = g( 2 );
    //        var g3 = g( 3 );
    //                    var g4 = g( 4 );
    //        var t = Test();
    //        var check = CompareClosures( t );
    //        var add = Add( t );
    //        var remove = Remove( t );
    //        t( clear );

    //                        //Setup:
    //                        //	1 -> 2 -> 3 -> 4
    //                        //	1 -> 4
    //                        //  plus, all empty loops

    //                        //Action: 
    //                        //	remove 1 -> 2

    //                        //Issue PMN-667: 
    //                        //	1 -> 4 also gets removed from the closure

    //        add( ce( g1, g1 ), ce( g2, g2 ), ce( g3, g3 ), ce( g4, g4 ), 
    //                        ce( g1, g2 ), ce( g2, g3 ), ce( g3, g4 ), ce( g1, g4 ) );
    //                    check( ce( g1, g1 ), ce( g2, g2 ), ce( g3, g3 ), ce( g4, g4 ),
    //                        ce( g1, g2, 1 ), ce( g2, g3, 1 ), ce( g3, g4, 1 ), ce( g2, g4, 2 ),
    //                        ce( g1, g4, 1 ), ce( g1, g3, 2 ) );

    //        remove( ce( g1, g2 ) );
    //                    check( ce( g1, g1 ), ce( g2, g2 ), ce( g3, g3 ), ce( g4, g4 ),
    //                        ce( g2, g3, 1 ), ce( g3, g4, 1 ), ce( g2, g4, 2 ),
    //                        ce( g1, g4, 1 ) );
    //            }

    //    [Fact]
    //    public void Security_DAG_DeleteTriggerShouldNotConsiderEmptyLoopAsAlternateRoute()
    //    {
    //        var g1 = g( 1 );
    //        var g2 = g( 2 );
    //        var g3 = g( 3 );
    //        var t = Test();
    //        var check = CompareClosures( t );
    //        var add = Add( t );
    //        var remove = Remove( t );
    //        t( clear );

    //        add( ce( g1, g2 ), ce( g1, g3 ), ce( g1, g1 ) );
    //        check( ce( g1, g2, 1 ), ce( g1, g3, 1 ), ce( g1, g1, 0 ) );

    //        remove( ce( g1, g3 ) );
    //        check( ce( g1, g2, 1 ), ce( g1, g1, 0 ) );
    //    }

    //    [Theory]
    //    [InlineData(0),InlineData(10),InlineData(44),InlineData(2),InlineData(9834), InlineData(11)]
    //    public void Security_DAG_Stress( int seed )
    //    {
    //        var t = Test();
    //        var check = CompareClosures( t );
    //        var add = Add( t );
    //        var remove = Remove( t );
    //        var rnd = new Random( seed );
    //        Func<Guid> rndg = () => g(rnd.Next(999)+1);

    //        var edges = new List<ClosureEdge>();
    //        var iterations = rnd.Next( 300 );

    //        t( clear );
    //        check();

    //        for ( var count = iterations; count >= 0; count-- )
    //        {
    //            switch ( rnd.Next( 2 ) )
    //            {
    //                case 0:
    //                    {
    //                        var es = Enumerable.Range( 0, rnd.Next( 10 ) ).Select( _ => ce( rndg(), rndg() ) ).Except( edges ).Distinct().ToArray();
    //                        es.ForEach( edges.Add );
    //                        add( es );
    //                        check( closure( edges ) );
    //                    }
    //                    break;

    //                case 1: 
    //                    if ( edges.Any() )
    //                    {
    //                        var es = Enumerable.Range( 0, rnd.Next( 10 ) ).Select( _ => rnd.Next( edges.Count ) ).Distinct().ToList();
    //                        remove( es.Select( i => edges[i] ).ToArray() );
    //                        foreach ( var i in es.OrderByDescending( x => x ) ) edges.RemoveAt( i );
    //                        check( closure( edges ) );
    //                    }
    //                    break;
    //            }
    //        }
    //    }

    //    static ClosureEdge[] closure( IList<ClosureEdge> es )
    //    {
    //        es = es.Select( e => new ClosureEdge { Start = e.Start, End = e.End, Distance = e.Start == e.End ? 0 : 1 } ).ToList();

    //        int oldCount;
    //        do
    //        {
    //            var newEdges = from a in es
    //                           join b in es on a.End equals b.Start
    //                           join t in es on new { a.Start, b.End } equals new { t.Start, t.End } into alreadyThere
    //                           let alreadyThereEdge = alreadyThere.FirstOrDefault()
    //                           where alreadyThereEdge == null || alreadyThereEdge.Distance > a.Distance + b.Distance
    //                           group a.Distance + b.Distance by new { a.Start, b.End } into variants
    //                           select new ClosureEdge { Start = variants.Key.Start, End = variants.Key.End, Distance = variants.Min() };

    //            oldCount = es.Count;
    //            es = es.Concat( newEdges.Distinct() ).ToList();
    //        }
    //        while ( oldCount < es.Count );

    //        return es.ToArray();

    //    }

    //    static Guid g( int idx )
    //    {
    //        return new Guid( "00000000-0000-0000-0000-000000000" + idx.ToString( "000" ) );
    //    }
    //    static ParamsDelegate<ClosureEdge> Add( Action<Action<Db>> ctx )
    //    {
    //        return ces => ctx( db => ces.ForEach( e => db.Edges.Add( new Data.MembershipEdge { Start = e.Start, End = e.End } ) ) );
    //    }
    //    static ParamsDelegate<ClosureEdge> Remove( Action<Action<Db>> ctx )
    //    {
    //        return ces => ctx( db => ces.ForEach( e => {
    //            var d = db.Edges.FirstOrDefault( x => x.Start == e.Start && x.End == e.End );
    //            if ( d != null ) db.Edges.Remove( d );
    //        } ) );
    //    }
    //    static void clear( Db db ) { foreach ( var e in db.Edges.ToList() ) db.Edges.Remove( e ); }
        
    //    Action<Action<Db>> Test( string table = null, string closureSuffix = null )
    //    {
    //        table = table ?? "SecurityInheritance";
    //        closureSuffix = closureSuffix ?? "";

    //        var b = new DbModelBuilder();
    //        b.Entity<Data.MembershipEdge>().HasKey( e => new { e.Start, e.End } ).ToTable( table );
    //        b.Entity<ClosureEdge>().HasKey( e => new { e.Start, e.End } ).ToTable( table + "Closure" + closureSuffix );
            
    //        DbCompiledModel m;
    //        using( var conn = DbFixture.OpenConnection() ) m = b.Build( conn ).Compile();

    //        return t =>
    //        {
    //            using( var db = new Db( DbFixture.ConnectionString, m ) ) 
    //            {
    //                t( db );
    //                db.SaveChanges();
    //            }
    //        };
    //    }

    //    delegate void ParamsDelegate<T>( params T[] es );
    //    ParamsDelegate<ClosureEdge> CompareClosures( Action<Action<Db>> t )
    //    {
    //        return es => t( db => Assert.Equal( 
    //            es.OrderBy( c => c.Start ).ThenBy( c => c.End ),
    //            db.Closures.AsEnumerable().OrderBy( c => c.Start ).ThenBy( c => c.End ) ) );
    //    }

    //    static ClosureEdge ce( Guid start, Guid end, int d = 0 ) { return new ClosureEdge { Start = start, End = end, Distance = d }; }

    //    class Db : DbContext
    //    {
    //        public DbSet<Data.MembershipEdge> Edges { get; set; }
    //        public DbSet<ClosureEdge> Closures { get; set; }
    //        public Db( string connString, DbCompiledModel m ) : base( connString, m ) {}
    //    }

    //    public void SetFixture( DatabaseFixture data )
    //    {
    //        DbFixture = data;
    //    }
    //}

    //class ClosureEdge : IComparable<ClosureEdge>, IEquatable<ClosureEdge>
    //{
    //    public Guid Start { get; set; }
    //    public Guid End { get; set; }
    //    public int Distance { get; set; }

    //    public int CompareTo( ClosureEdge other )
    //    {
    //        if ( other == null ) return 1;
    //        var r = Start.CompareTo( other.Start );
    //        if ( r == 0 ) r = End.CompareTo( other.End );
    //        if ( r == 0 ) r = Distance.CompareTo( other.Distance );
    //        return r;
    //    }

    //    public override string ToString()
    //    {
    //        return string.Format( "{0} -> {1} x {2}", Start.ToString().Substring( 33 ), End.ToString().Substring( 33 ), Distance );
    //    }

    //    public bool Equals( ClosureEdge other )
    //    {
    //        return this.CompareTo( other ) == 0;
    //    }

    //    public override bool Equals( object obj )
    //    {
    //        return this.Equals( obj as ClosureEdge );
    //    }

    //    public override int GetHashCode()
    //    {
    //        return Start.GetHashCode() ^ End.GetHashCode() ^ Distance;
    //    }
    //}
}