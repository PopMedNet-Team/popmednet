using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using Lpp.Composition;
using System.Diagnostics.Contracts;

namespace Lpp.Data.Composition
{
    [Export, PartMetadata( ExportScope.Key, TransactionScope.Id )]
    public class DatabaseBootstrap
    {
        [ImportMany] public IEnumerable<Lazy<IDatabaseBootstrapSegment>> Segments { get; set; }
        [Import] public ICompositionService Composition { get; set; }
        private Lazy<ILookup<Type, IDatabaseBootstrapSegment>> _segments;

        public DatabaseBootstrap()
        {
            _segments = new Lazy<ILookup<Type, IDatabaseBootstrapSegment>>( () => Segments.ToLookup( s => s.Value.Domain, s => s.Value ) );
        }

        public IEnumerable<IDatabaseDomain> Domains
        {
            get { return _segments.Value.Select( d => new DbDomain( this, d.Key ) ); }
        }

        class DbDomain : IDatabaseDomain
        {
            private readonly Lazy<DbContext> _dbContext;
            private readonly DatabaseBootstrap _root;
            public Type Domain { get; private set; }

            public DbDomain( DatabaseBootstrap root, Type domain )
            {
                _root = root;
                Domain = domain;
                _dbContext = new Lazy<DbContext>( () =>
                    _root.Composition.Get( typeof( IDbContext<> ).MakeGenericType( Domain ) ) as DbContext );
                //Contract.Assume( _dbContext != null );
            }

            public bool ConnectionOk
            {
                get { return _dbContext.Value.Database.Exists(); }
            }

            public void CreateDatabase()
            {
//                _dbContext.Value.Database.Delete();
                _dbContext.Value.Database.Create();
                foreach ( var s in _root._segments.Value[Domain] ) s.Execute( _dbContext.Value );
            }
        }
    }

    public interface IDatabaseDomain
    {
        Type Domain { get; }
        bool ConnectionOk { get; }
        void CreateDatabase();
    }
}