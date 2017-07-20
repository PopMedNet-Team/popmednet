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

namespace Lpp.Security.Tests
{
    //public class SqlHierarchy : IUseFixture<DatabaseFixture>
    //{
    //    DatabaseFixture DbFixture { get; set; }

    //    [Fact]
    //    public void Security_TreeSync_Basic()
    //    {
    //        Test( x =>
    //        {
    //            x.AssertDatabaseTreesCorrect();

    //            var id1 = x.Add( null ); x.AssertDatabaseTreesCorrect();
    //            var id2 = x.Add( null ); x.AssertDatabaseTreesCorrect();
    //            var id3 = x.Add( null ); x.AssertDatabaseTreesCorrect();

    //            x.Remove( id2 ); x.AssertDatabaseTreesCorrect();

    //            var id4 = x.Add( null ); x.AssertDatabaseTreesCorrect();
    //            var id5 = x.Add( null ); x.AssertDatabaseTreesCorrect();

    //            x.Move( id1, id4 ); x.AssertDatabaseTreesCorrect();
    //            x.Move( id3, id4 ); x.AssertDatabaseTreesCorrect();
    //            x.Move( id4, id5 ); x.AssertDatabaseTreesCorrect();

    //            x.Move( id3, null ); x.AssertDatabaseTreesCorrect();
    //            x.Move( id4, null ); x.AssertDatabaseTreesCorrect();
    //        } );
    //    }

    //    [Fact]
    //    public void Security_TreeSync_RemoveHive()
    //    {
    //        Test( x =>
    //        {
    //            var root = x.Add( null );
    //            for ( int i = 0; i < 3; i++ )
    //            {
    //                var a = x.Add( root );
    //                for ( int j = 0; j < 5; j++ )
    //                {
    //                    var b = x.Add( a );
    //                    for ( int k = 0; k < 3; k++ ) x.Add( b );
    //                }
    //            }

    //            x.AssertDatabaseTreesCorrect();

    //            x.Remove( x.Tree.Children[0].Children[0].Node );
    //            x.AssertDatabaseTreesCorrect();

    //            x.Remove( x.Tree.Children[0].Children[0].Node );
    //            x.AssertDatabaseTreesCorrect();
    //        } );
    //    }

    //    [Fact]
    //    public void Security_TreeSync_MoveHive()
    //    {
    //        Test( x =>
    //        {
    //            var root = x.Add( null );
    //            for ( int i = 0; i < 3; i++ )
    //            {
    //                var a = x.Add( root );
    //                for ( int j = 0; j < 5; j++ )
    //                {
    //                    var b = x.Add( a );
    //                    for ( int k = 0; k < 3; k++ ) x.Add( b );
    //                }
    //            }

    //            x.AssertDatabaseTreesCorrect();

    //            x.Move( x.Tree.Children[0].Children[0].Node, x.Tree.Children[0].Children[1].Node );
    //            x.AssertDatabaseTreesCorrect();

    //            x.Move( x.Tree.Children[0].Children[1].Node, x.Tree.Children[0].Children[0].Children[1].Node );
    //            x.AssertDatabaseTreesCorrect();
    //        } );
    //    }

    //    //[Theory]
    //    //public void Security_TreeSync_Random()
    //    //{
    //    //}

    //    void Test( Action<Ctx> t )
    //    {
    //        using ( var db = new Db( DbFixture.ConnectionString ) )
    //        {
    //            db.Objects.ToList().Select( db.Objects.Remove ).LastOrDefault();
    //            db.SaveChanges();
    //        }

    //        using ( var db = new Db( DbFixture.ConnectionString ) )
    //        {
    //            var tree = new Tree<Guid>();
    //            t( new Ctx { Tree = tree, Db = db, ConnectionString = DbFixture.ConnectionString } );
    //        }
    //    }

    //    class Ctx
    //    {
    //        public string ConnectionString { get; set; }
    //        public Tree<Guid> Tree { get; set; }
    //        public Db Db { get; set; }

    //        public Guid Add( Guid? parent )
    //        {
    //            var id = Guid.NewGuid();
    //            Db.Objects.Add( new Data.SecurityObject { Id = id, Parent = parent == null ? null : Db.Objects.Find( parent.Value ) } );
    //            Db.SaveChanges();
    //            (parent == null ? Tree : Tree.Find( parent.Value )).Children.Add( new Tree<Guid> { Node = id } );
    //            return id;
    //        }

    //        public void Remove( Guid id )
    //        {
    //            var o = Db.Objects.Find( id );
    //            Assert.NotNull( o );
    //            Db.Objects.Remove( o );
    //            Db.SaveChanges();

    //            var removed = Tree.Remove( id );
    //            Tree.Children.AddRange( removed.Children );
    //        }

    //        public void Move( Guid id, Guid? newParent )
    //        {
    //            var o = Db.Objects.Find( id );
    //            Assert.NotNull( o );
    //            o.Parent = newParent == null ? null : Db.Objects.Find( newParent.Value );
    //            Db.SaveChanges();

    //            var removed = Tree.Remove( id );
    //            (newParent == null ? Tree : Tree.Find( newParent.Value )).Children.Add( removed );
    //        }

    //        public void AssertDatabaseTreesCorrect()
    //        {
    //            using ( var db = new Db( ConnectionString ) )
    //            {
    //                var ntree = Tree.NumerateChildren();
    //                Assert.Equal(
    //                    ntree.OrderBy( o => o.Node.Item1 ).Select( o => o.Node ),
    //                    db.Objects.Where( o => o.Parent == null ).AsEnumerable()
    //                    .OrderBy( o => o.Id ).Select( o => Tuple.Create( o.Id, o.LeftIndex, o.RightIndex ) ) );

    //                Action<Tree<Tuple<Guid, int, int>>, Data.SecurityObject> assertOneSubtree = null;
    //                assertOneSubtree = ( subTree, obj ) =>
    //                {
    //                    Assert.Equal(
    //                        subTree.Children.Select( c => c.Node.Item1 ).OrderBy( x => x ),
    //                        obj.Children.Select( c => c.Id ).OrderBy( x => x )
    //                    );
    //                    Assert.Equal(
    //                        subTree.Children.OrderBy( c => c.Node.Item2 ).Select( c => Tuple.Create( c.Node.Item2, c.Node.Item3 ) ),
    //                        obj.Children.OrderBy( c => c.LeftIndex ).Select( c => Tuple.Create( c.LeftIndex, c.LeftIndex ) )
    //                    );

    //                    (from m in subTree.Children
    //                     join d in obj.Children
    //                     on m.Node.Item1 equals d.Id
    //                     select new { m, d }
    //                    )
    //                    .ForEach( x => assertOneSubtree( x.m, x.d ) );
    //                };
    //            }
    //        }
    //    }

    //    class Db : DbContext
    //    {
    //        public DbSet<Data.SecurityObject> Objects { get; set; }
    //        public Db( string connStr ) : base( connStr ) { }
    //        protected override void OnModelCreating( DbModelBuilder modelBuilder )
    //        {
    //            new Data.SecurityPersistenceDefinition<int>().BuildModel( modelBuilder );
    //        }
    //    }

    //    class Tree<T>
    //    {
    //        public T Node { get; set; }
    //        public List<Tree<T>> Children { get; private set; }
    //        public Tree() : this( new List<Tree<T>>() ) { }
    //        private Tree( List<Tree<T>> children ) { Children = children; }

    //        public IEnumerable<Tree<Tuple<T, int, int>>> NumerateChildren()
    //        {
    //            return Children.Select( c => c.NumerateNodes( 0 ) );
    //        }

    //        public Tree<T> Find( T node ) { return Equals( Node, node ) ? this : Children.Select( c => c.Find( node ) ).FirstOrDefault( f => f != null ); }
    //        public Tree<T> Remove( T node )
    //        {
    //            if ( Equals( Node, node ) ) Assert.True( false );

    //            return Children.ToList().Select( ( c, i ) =>
    //            {
    //                if ( Equals( c.Node, node ) ) { Children.RemoveAt( i ); return c; }
    //                return c.Remove( node );
    //            } )
    //                .Where( r => r != null )
    //                .FirstOrDefault();
    //        }

    //        public Tree<Tuple<T, int, int>> NumerateNodes( int leftIndex )
    //        {
    //            var numChildren = 
    //                Children
    //                .Aggregate( new List<Tree<Tuple<T, int, int>>>(), ( l, c ) =>
    //                {
    //                    var prev = l.LastOrDefault();
    //                    var nc = c.NumerateNodes( prev == null ? leftIndex+1 : prev.Node.Item3+1 );
    //                    l.Add( nc );
    //                    return l;
    //                } );

    //            var last = numChildren.LastOrDefault();
    //            return new Tree<Tuple<T, int, int>>( numChildren ) { Node = Tuple.Create( Node, leftIndex, last == null ? leftIndex+1 : last.Node.Item3+1 ) };
    //        }

    //        public IEnumerable<T> AllChildren()
    //        {
    //            return new[] { Node }.Concat( Children.SelectMany( c => c.AllChildren() ) );
    //        }
    //    }

    //    public void SetFixture( DatabaseFixture data )
    //    {
    //        DbFixture = data;
    //    }
    //}
}