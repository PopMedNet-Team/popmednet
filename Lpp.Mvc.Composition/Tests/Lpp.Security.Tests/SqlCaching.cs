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
//using Xunit.Extensions;
using System.Data.SqlClient;
using Lpp.Security.Data;
using System.Diagnostics;

namespace Lpp.Security.Tests
{
    //public class SqlCaching : IUseFixture<DatabaseFixture>
    //{
    //    DatabaseFixture DbFixture { get; set; }

    //    [Theory]
    //    [InlineData( 1 )]
    //    [InlineData( 2 )]
    //    [InlineData( 3 )]
    //    [InlineData( 4 )]
    //    [InlineData( 5 )]
    //    public void Security_SecurityObjects_Copy_Sync( int randomSeed )
    //    {
    //        using ( var db = new Db( DbFixture.ConnectionString ) )
    //        {
    //            db.Database.ExecuteSqlCommand( "delete from securityobjects" );
    //            db.SaveChanges();
    //        }

    //        Debug.WriteLine( "Current time: {0:HH:mm:ss.ffff}", DateTime.Now );
    //        var rnd = new Random( randomSeed );
    //        var count = Math.Abs( rnd.Next( 500 ) );
    //        while ( count-- > 0 )
    //        {
    //            Manipulate( rnd );
    //            Assert();
    //        }
    //    }

    //    void Manipulate( Random rnd )
    //    {
    //        using ( var db = new Db( DbFixture.ConnectionString ) )
    //        {
    //            var a = rnd.Next( 5 );
    //            Debug.WriteLine( "Action: " + a );
    //            switch ( a )
    //            {
    //                case 0: db.Objects.Add( new SecurityObject { Id = Guid.NewGuid(), Parent = Parent(db, rnd), TreeTag = Guid.NewGuid() } ); break;
    //                case 1: db.Objects.Add( new SecurityObject { Id = Guid.NewGuid(), Parent = Parent(db, rnd) } ); break;
    //                case 2: db.Objects.Add( new SecurityObject { Id = Guid.NewGuid() } ); break;
    //                case 3: db.Objects.OrderBy( o => o.Id ).Skip( rnd.Next( db.Objects.Count() ) ).Take( rnd.Next( 10 ) ).ForEach( o => 
    //                    { o.Children.Clear(); db.Objects.Remove( o ); } ); break;
    //                case 4: db.Objects.OrderBy( o => o.Id ).Skip( rnd.Next( db.Objects.Count() ) ).Take( rnd.Next( 10 ) ).ForEach( o => 
    //                    o.Parent = Parent( db, rnd ) ); break;
    //            }
    //            db.SaveChanges();
    //        }
    //    }

    //    SecurityObject Parent( Db db, Random rnd )
    //    {
    //        var p = db.Objects.OrderBy( o => o.Id ).Skip( rnd.Next( 500 ) ).FirstOrDefault();
    //        Debug.WriteLine( p == null ? "Parent: null" : "Parent: " + p.Id );
    //        return p;
    //    }

    //    void Assert()
    //    {
    //        ForAllDbs( db =>
    //        {
    //            var different = from o in db.Objects
    //                            join o2 in db.Objects2 on o.Id equals o2.Id into o2s
    //                            from o2 in o2s.DefaultIfEmpty()
    //                            where o2 == null || o.LeftIndex != o2.LeftIndex || o.RightIndex != o2.RightIndex || o.TreeTag != o2.TreeTag
    //                            select new { o, o2 };
    //            Xunit.Assert.True( !different.Any(), "Mistamtching copies: " + string.Join( Environment.NewLine,
    //                different.AsEnumerable().Select( d =>
    //                    string.Format( "[ id: {0}, li: {1} / {2}, ri: {3} / {4}, tt: {5} / {6} ]",
    //                    d.o.Id, d.o.LeftIndex, d.o2.LeftIndex, d.o.RightIndex, d.o2.RightIndex, d.o.TreeTag, d.o2.TreeTag
    //            ) ) ) );

    //            var extra = from o2 in db.Objects2
    //                        join o in db.Objects on o2.Id equals o.Id into os
    //                        where !os.Any()
    //                        select o2.Id;
    //            Xunit.Assert.True( !extra.Any(), "Orphan copies: " + string.Join( ", ", extra ) );
    //        } );
    //    }

    //    void ForAllDbs( Action<Db> action )
    //    {
    //        ForDb( () => new Db( DbFixture.ConnectionString ), action );
    //        ForDb( () => new Db2p( DbFixture.ConnectionString ), action );
    //        ForDb( () => new Db2c( DbFixture.ConnectionString ), action );
    //        ForDb( () => new Db3p( DbFixture.ConnectionString ), action );
    //        ForDb( () => new Db3c( DbFixture.ConnectionString ), action );
    //        ForDb( () => new Db4p( DbFixture.ConnectionString ), action );
    //        ForDb( () => new Db4c( DbFixture.ConnectionString ), action );
    //        ForDb( () => new Db5p( DbFixture.ConnectionString ), action );
    //        ForDb( () => new Db5c( DbFixture.ConnectionString ), action );
    //    }

    //    void ForDb( Func<Db> createDb, Action<Db> action )
    //    {
    //        using ( var db = createDb() ) action( db );
    //    }

    //    public class Db : DbContext
    //    {
    //        public virtual string Suffix { get { return "1_p"; } }

    //        public DbSet<SecurityObject> Objects { get; set; }
    //        public DbSet<SecObject2> Objects2 { get; set; }

    //        public Db( string connStr ) : base( connStr ) { }
    //        protected override void OnModelCreating( DbModelBuilder b )
    //        {
    //            b.Entity<SecurityObject>().HasKey( o => o.Id );
    //            b.Entity<SecurityObject>().HasOptional( o => o.Parent ).WithMany( o => o.Children ).Map( m => m.MapKey( "ParentId" ) );

    //            b.Entity<SecObject2>().HasKey( o => o.Id ).ToTable( "SecurityObjects" + Suffix ) ;
    //        }
    //    }

    //    public class Db2c : Db { public override string Suffix { get { return "2_c"; } } public Db2c( string connStr ) : base( connStr ) { } }
    //    public class Db2p : Db { public override string Suffix { get { return "2_p"; } } public Db2p( string connStr ) : base( connStr ) { } }
    //    public class Db3c : Db { public override string Suffix { get { return "3_c"; } } public Db3c( string connStr ) : base( connStr ) { } }
    //    public class Db3p : Db { public override string Suffix { get { return "3_p"; } } public Db3p( string connStr ) : base( connStr ) { } }
    //    public class Db4c : Db { public override string Suffix { get { return "4_c"; } } public Db4c( string connStr ) : base( connStr ) { } }
    //    public class Db4p : Db { public override string Suffix { get { return "4_p"; } } public Db4p( string connStr ) : base( connStr ) { } }
    //    public class Db5c : Db { public override string Suffix { get { return "5_c"; } } public Db5c( string connStr ) : base( connStr ) { } }
    //    public class Db5p : Db { public override string Suffix { get { return "5_p"; } } public Db5p( string connStr ) : base( connStr ) { } }

    //    public void SetFixture( DatabaseFixture data )
    //    {
    //        DbFixture = data;
    //    }
    //}

    public class SecObject2
    {
        public Guid Id { get; set; }
        public int LeftIndex { get; set; }
        public int RightIndex { get; set; }
        public Guid? TreeTag { get; set; }
    }
}