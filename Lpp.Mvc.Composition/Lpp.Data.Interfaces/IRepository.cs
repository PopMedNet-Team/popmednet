using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Data
{
    public interface IRepository
    {
        IQueryable All { get; }
        void Clear();
    }

    public interface IRepository<TEntity> : IRepository where TEntity : class
    {
        TEntity Find( params object[] key );
        new IQueryable<TEntity> All { get; }
        TEntity Find( object key );
        TEntity Add( TEntity e );
        TEntity Attach(TEntity e);
        TEntity Attach(TEntity e, bool changed);
        void Remove( TEntity e );
    }

    public interface IRepository<TDomain,TEntity> : IRepository<TEntity> where TEntity : class
    {
    }
}