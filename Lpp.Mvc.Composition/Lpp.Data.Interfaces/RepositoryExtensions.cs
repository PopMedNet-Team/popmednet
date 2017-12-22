using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Lpp.Utilities.Legacy;

namespace Lpp.Data
{
    public static class RepositoryExtensions
    {
        public static MaybeNotNull<TEntity> MaybeFind<TDomain, TEntity>( this IRepository<TDomain, TEntity> repo, params object[] key ) 
            where TEntity : class
        {
            //Contract.Requires(repo != null);
            return Maybe.Value( repo.Find( key ) );
        }
    }
}