using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.ComponentModel.Composition;
using Lpp.Composition;

namespace Lpp.Data.Composition
{  
    [Export( typeof( IRepository<,> ) )]
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    [PartCreationPolicy( CreationPolicy.Shared )]
    public class Repository<TDomain,TEntity> : IRepository<TDomain,TEntity>, IRepository
        where TEntity : class
    {
        private IDbContext<TDomain> _context;
        public void Dispose()
        {
            this._context = null;
        }

        DbSet<TEntity> MySet
        {
            get
            {
                return _context.Set<TEntity>();
            }
        }

        [ImportingConstructor]
        internal Repository( IDbContext<TDomain> context )
        {
            //Contract.Requires( context != null );
            _context = context;
        }

        public TEntity Find( params object[] key ) { return MySet.Find( key ); }
        public IQueryable<TEntity> All { get { return MySet; } }
        IQueryable IRepository.All { get { return All; } }
        public TEntity Add( TEntity e ) { return MySet.Add( e ); }
        public TEntity Attach(TEntity e) { return MySet.Attach(e); }
        public TEntity Attach(TEntity e, bool changed)
        {
            if (!changed)
                return Attach(e);

            return null;
        }
        public void Remove( TEntity e ) { MySet.Remove( e ); }
        public void Clear() { foreach (TEntity e in MySet) Remove(e); }
        public TEntity Find( object key ) { return MySet.Find( key ); }
    }
}