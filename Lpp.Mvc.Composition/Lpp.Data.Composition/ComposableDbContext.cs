using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using Lpp.Composition;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Web;
using Lpp.Utilities.Legacy;

namespace Lpp.Data.Composition
{
    [Export( typeof( IDbContext<> ) )]
    [Export( typeof( IUnitOfWork<> ) )]
    [Export( typeof( IPersistenceMagic<> ) )]
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class UnitOfWork<TDomain> : IDisposable, IDbContext<TDomain>, IUnitOfWork<TDomain>, IPersistenceMagic<TDomain>
    {
        private ComposableDbContext<TDomain> _inner;
        private readonly PersistenceConfig<TDomain> _config;
        private readonly IEnumerable<Lazy<IPersistenceDefinition<TDomain>>> _definitions;

        [ImportingConstructor]
        public UnitOfWork( 
            [Import(AllowDefault=true, RequiredCreationPolicy = CreationPolicy.Shared)] PersistenceConfig<TDomain> config,
            [ImportMany(RequiredCreationPolicy = CreationPolicy.Shared)] IEnumerable<Lazy<IPersistenceDefinition<TDomain>>> definitions )
        {
            _config = config ?? new PersistenceConfig<TDomain>();
            _definitions = definitions.EmptyIfNull();
            CreateInner();
        }

        void CreateInner()
        {
            if (HttpContext.Current != null && HttpContext.Current.Server != null)
            {
                if (HttpContext.Current.Items["_inner" + typeof(TDomain).FullName] != null)
                {
                    this._inner = (ComposableDbContext<TDomain>)HttpContext.Current.Items["_inner" + typeof(TDomain).FullName];
                    return;
                }
            }

            _inner = _config.CreateConnection == null
                ? new ComposableDbContext<TDomain>(_config.ConnectionString ?? typeof(TDomain).FullName, BuildModel)
                : new ComposableDbContext<TDomain>(CreateConnection(), BuildModel);
            if (HttpContext.Current != null && HttpContext.Current.Server != null)
            {
                HttpContext.Current.Items.Add("_inner" + typeof(TDomain).FullName, _inner);
            }
        }

        DbConnection CreateConnection()
        {
            var c = _config.CreateConnection();
            if ( !_config.ConnectionString.NullOrEmpty() ) c.ConnectionString = _config.ConnectionString;
            return c;
        }
        void BuildModel( DbModelBuilder mb ) { foreach ( var d in _definitions ) d.Value.BuildModel( mb ); }

        public void Commit() { EnsureInitialized().SaveChanges(); }
        public void Rollback() { _inner.Dispose(); CreateInner(); }
        public void Dispose() { _inner.Dispose(); }
        public T AttachEntity<T>( T entity ) where T : class { return EnsureInitialized().Set<T>().Attach( entity ); }
        public void SetModified<T>( T entity ) where T : class { EnsureInitialized().Entry( entity ).State = EntityState.Modified; }
        public ObjectContext ObjectContext { get { return (EnsureInitialized() as IObjectContextAdapter).ObjectContext; } }
        public DbSet<T> Set<T>() where T : class { return EnsureInitialized().Set<T>(); }

        private ComposableDbContext<TDomain> EnsureInitialized()
        {
            return _inner;
        }
    }

    public class ComposableDbContext<TDomain> : DbContext
    {
        private readonly Action<DbModelBuilder> _onModelCreating;

        public ComposableDbContext( DbConnection conn, Action<DbModelBuilder> onModelCreating )
            : base( conn, true )
        {
            this.Database.CommandTimeout = Int32.MaxValue;

            _onModelCreating = onModelCreating;
        }
        public ComposableDbContext( string nameOrConnectionString, Action<DbModelBuilder> onModelCreating )
            : base( nameOrConnectionString )
        {
            this.Database.CommandTimeout = Int32.MaxValue;

            _onModelCreating = onModelCreating;
        }
        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            if (_onModelCreating != null) //This checks for null because DataContext passes null because it handles it's own model creation.
                _onModelCreating( modelBuilder );
        }

        public override int SaveChanges()
        {
            int count = 0;
        retry:

            try
            {
                return base.SaveChanges();
            }
            catch (CommitFailedException ce)
            {
                count++;
                if (count <= 5)
                {
                    System.Threading.Thread.Sleep(300 * count);
                    goto retry;
                }

                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    internal interface IDbContext<TDomain> : IObjectContextAdapter
    {
        DbSet<T> Set<T>() where T : class;
    }
}