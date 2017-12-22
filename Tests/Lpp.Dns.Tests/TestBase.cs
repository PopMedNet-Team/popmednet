using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Lpp.Composition;
using Lpp.Dns.Model;
using Lpp.Security;
using Lpp.Tests;
//using Xunit;

namespace Lpp.Dns.Tests
{
    //public class TestBase : IUseFixture<DatabaseFixture>, IUseFixture<CompositionFixture>
    //{
    //    public CompositionContainer OpenScope( string id = null )
    //    {
    //        return new MockScopeBuilder( Composition )
    //            .Module( Sec.Module<DnsDomain>() )
    //            .Build();
    //    }

    //    public T Tran<T>( Func<ICompositionService, T> t )
    //    {
    //        using ( var scope = OpenScope( TransactionScope.Id ) ) return t( scope );
    //    }

    //    public void Tran( Action<ICompositionService> t )
    //    {
    //        Tran( scope => { t( scope ); return 0; } );
    //    }

    //    public void DbTran( Action<ICompositionService> t )
    //    {
    //        DbTran( scope => { t( scope ); return 0; } );
    //    }

    //    public T DbTran<T>( Func<ICompositionService, T> t )
    //    {
    //        throw new Lpp.Utilities.CodeToBeUpdatedException();

    //        //return Tran( scope =>
    //        //{
    //        //    var res = t( scope );
    //        //    scope.Get<IUnitOfWork<DnsDomain>>().Commit();
    //        //    return res;
    //        //} );
    //    }

    //    public DatabaseFixture Database { get; private set; }
    //    public CompositionFixture Composition { get; private set; }

    //    void IUseFixture<DatabaseFixture>.SetFixture( DatabaseFixture data )
    //    {
    //        Database = data;
    //        Init();
    //    }

    //    void IUseFixture<CompositionFixture>.SetFixture( CompositionFixture data )
    //    {
    //        Composition = data;
    //        Init();
    //    }

    //    void Init()
    //    {
    //        throw new Lpp.Utilities.CodeToBeUpdatedException();

    //        //if ( Database != null && Composition != null )
    //        //{
    //        //    var configs = Composition.RootContainer.GetMany<PersistenceConfig<DnsDomain>>();
    //        //    if ( configs.Any() )
    //        //    {
    //        //        configs.First().ConnectionString = Database.ConnectionString;
    //        //    }
    //        //    else
    //        //    {
    //        //        Composition.RootContainer.ComposeExportedValue(
    //        //            new PersistenceConfig<DnsDomain> { ConnectionString =  Database.ConnectionString } );
    //        //    }
    //        //}
    //    }
    //}
}