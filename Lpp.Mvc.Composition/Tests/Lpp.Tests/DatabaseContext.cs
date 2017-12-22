using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Lpp.Composition;

namespace Lpp.Tests
{
    //public abstract class DatabaseFixture : IDisposable
    //{
    //    public static string ServerName = ".";
    //    public readonly static string[] TryServers = new[] { ".", ".\\sqlexpress" };

    //    public string DatabaseName { get; private set; }
    //    public string ConnectionString { get { return string.Format( "Server={0};database={1};integrated security=true;MultipleActiveResultSets=True", ServerName, DatabaseName ); } }

    //    public DatabaseFixture()
    //    {
    //        DatabaseName = "Test_" + Guid.NewGuid().ToString().Replace( "-", "" );

    //        try { using ( var conn = OpenConnection() ) { ExecuteCommand( conn, Properties.Resources.DropTestDatabases ); } }
    //        catch { }

    //        using ( var conn = OpenConnection() )
    //        {
    //            new[] {
    //                string.Format( "create database [{0}]", DatabaseName ),
    //                string.Format( "use [{0}]", DatabaseName )
    //            }
    //            .Concat( DatabaseCreateScript() )
    //            .Where( s => !string.IsNullOrEmpty( s ) )

    //            .ForEach( cmd => ExecuteCommand( conn, cmd ) );
    //        }
    //    }

    //    protected abstract IEnumerable<string> DatabaseCreateScript();

    //    public virtual void Dispose()
    //    {
    //        using ( var conn = OpenConnection() )
    //        {
    //            try { ExecuteCommand( conn, string.Format( "drop database [{0}]", DatabaseName ) ); }
    //            catch { }
    //        }
    //    }

    //    public void ExecuteCommand( SqlConnection conn, string command )
    //    {
    //        Debug.WriteLine( command );
    //        using ( var cmd = conn.CreateCommand() )
    //        {
    //            cmd.CommandText = command;
    //            cmd.ExecuteNonQuery();
    //        }
    //    }

    //    public SqlConnection OpenConnection()
    //    {
    //        var tryInOrder = TryServers.Where( s => s != ServerName ).StartWith( ServerName );
    //        var result =  
    //            tryInOrder
    //            .Select( OpenConnection )
    //            .Where( c => c != null )
    //            .Do( c => ServerName = c.DataSource )
    //            .FirstOrDefault();

    //        if ( result == null ) throw new Exception( "Cannot open a connection to SQL Server. Servers attempted: " + 
    //            string.Join( ", ", tryInOrder ) );
    //        return result;
    //    }

    //    SqlConnection OpenConnection( string serverName )
    //    {
    //        try
    //        {
    //            var conn = new SqlConnection( string.Format( "server={0};integrated security=true", serverName ) );
    //            conn.Open();
    //            return conn;
    //        }
    //        catch( Exception ex )
    //        {
    //            Debug.WriteLine( ex.ToString() );
    //            return null;
    //        }
    //    }

    //    protected IEnumerable<string> Statements( string code )
    //    {
    //        return ((DbContext)null).SplitStatements( code );
    //    }
    //}

    //public class IntegrationFixture<TDbFixture, TDomain>
    //    where TDbFixture : DatabaseFixture, new()
    //{
    //    public CompositionFixture Composition { get; private set; }
    //    public TDbFixture Db { get; private set; }

    //    public IntegrationFixture()
    //    {
    //        Db = new TDbFixture();
    //        Composition = new CompositionFixture( b => b.AddExportedValue( new PersistenceConfig<TDomain> { ConnectionString = Db.ConnectionString } ) );
    //    }

    //    public MockScopeBuilder ScopeBuilder() { return new MockScopeBuilder( Composition ); }
    //    public CompositionContainer Scope( string id = null, Action<MockScopeBuilder> setup = null )
    //    {
    //        var b = ScopeBuilder();
    //        if ( setup != null ) setup( b );
    //        return b.Build( id );
    //    }
    //    public CompositionContainer Scope( Action<MockScopeBuilder> setup ) { return Scope( null, setup ); }
    //}

    //public static class TestExtensions
    //{
    //    public static T Tran<T, TDbFixture, TDomain>( this IntegrationFixture<TDbFixture, TDomain> i, Func<CompositionContainer, T> action )
    //        where TDbFixture : DatabaseFixture, new()
    //    {
    //        using ( var scope = i.Scope() )
    //        {
    //            var res = action( scope );
    //            scope.Get<IUnitOfWork<TDomain>>().Commit();
    //            return res;
    //        }
    //    }

    //    public static void Tran<TDbFixture, TDomain>( this IntegrationFixture<TDbFixture, TDomain> i, Action<CompositionContainer> action )
    //        where TDbFixture : DatabaseFixture, new()
    //    {
    //        i.Tran( s => { action( s ); return 0; } );
    //    }

    //    public static IRepository<TDomain, TEntity> Repo<TDomain, TEntity>( this CompositionContainer c )
    //        where TEntity : class
    //    {
    //        return c.Get<IRepository<TDomain, TEntity>>();
    //    }
    //}
}