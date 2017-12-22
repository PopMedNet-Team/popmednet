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
using Lpp.Composition.Modules;

namespace Lpp.Tests
{
    //public static class Mock
    //{
    //    public static DataMock<TDomain> Data<TDomain>()
    //    {
    //        return DataMock<TDomain>.Instance;
    //    }

    //    public static MockScopeBuilder MockScope( this CompositionFixture comp )
    //    {
    //        return new MockScopeBuilder( comp );
    //    }
    //}

    //public class MockScopeBuilder
    //{
    //    private readonly CompositionFixture _comp;
    //    private readonly CompositionBatch _overrides = new CompositionBatch();
    //    private readonly List<IModule> _modules = new List<IModule>();

    //    public MockScopeBuilder( CompositionFixture comp )
    //    {
    //        _comp = comp;
    //    }

    //    public MockScopeBuilder Override<T>( T export )
    //    {
    //        _overrides.AddExportedValue( export );
    //        return this;
    //    }

    //    public MockScopeBuilder Override<T>( string contractName, T export )
    //    {
    //        _overrides.AddExportedValue( contractName, export );
    //        return this;
    //    }

    //    public MockScopeBuilder Module( IModule module )
    //    {
    //        _modules.Add( module );
    //        return this;
    //    }

    //    public CompositionContainer Build( string id = null )
    //    {
    //        var overrides = new ComposablePartExportProvider();
    //        var exps = new ComposablePartExportProvider();
    //        var defs = _modules.SelectMany( m => m.GetDefinition() ).ToList();
    //        var modules = new CatalogExportProvider( new AggregateCatalog( defs.Select( d => d.Catalog ) ) );
            
    //        var res = _comp.Scoping.OpenScope( new[] { id ?? TransactionScope.Id }, new ExportProvider[] { overrides, modules, exps } );
    //        overrides.SourceProvider = res;
    //        modules.SourceProvider = res;
    //        exps.SourceProvider = res;

    //        var b = new CompositionBatch();
    //        defs.ForEach( d => d.ExplicitExports( b ) );
    //        exps.Compose( b );

    //        overrides.Compose( _overrides );

    //        return res;
    //    }
    //}

    //public class DataMock<TDomain> : IUnitOfWork<TDomain>
    //{
    //    public static readonly DataMock<TDomain> Instance = new DataMock<TDomain>();

    //    public void Dispose()
    //    {
            
    //    }

    //    public IRepository<TDomain, TEntity> Repository<TEntity>(Func<TEntity, object> key, params TEntity[] entities)
    //        where TEntity : class
    //    {
    //        return new ExplicitRepository<TEntity>( entities, key );
    //    }

    //    public IUnitOfWork<TDomain> UnitOfWork() { return this; }
    //    public void Commit() { }
    //    public void Rollback() { }

    //    class ExplicitRepository<TEntity> : IRepository<TDomain, TEntity>
    //        where TEntity : class
    //    {
    //        private readonly List<TEntity> _entities;
    //        private readonly Func<TEntity, object> _key;

    //        public ExplicitRepository( IEnumerable<TEntity> ents, Func<TEntity, object> key )
    //        {
    //            _entities = ents.ToList();
    //            _key = key;
    //        }

    //        public TEntity Add( TEntity e )
    //        {
    //            _entities.Add( e );
    //            return e;
    //        }

    //        public IQueryable<TEntity> All
    //        {
    //            get { return _entities.AsQueryable(); }
    //        }
    //        IQueryable IRepository.All { get { return All; } }

    //        public TEntity Find( object key )
    //        {
    //            return _entities.FirstOrDefault( e => Equals( _key( e ), key ) );
    //        }

    //        public TEntity Find( params object[] key )
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public void Remove( TEntity e )
    //        {
    //            _entities.Remove( e );
    //        }

    //        public void Clear()
    //        {
    //            throw new NotImplementedException();
    //        }




    //        public TEntity Attach(TEntity e)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public TEntity Attach(TEntity e, bool changed)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }
    //}
}