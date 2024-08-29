using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel.Composition;
//using Lpp.Data;
using Lpp.Composition;

namespace Lpp.Security
{
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    public class RepositorySecObjectProvider<TDomain,TEntity> : ISecurityObjectProvider<TDomain>
        where TEntity : class, ISecurityObject
    {
        //[Import] public IRepository<TDomain,TEntity> Repo { get; set; }
        //private readonly Func<IRepository<TDomain, TEntity>, Guid, TEntity> _find;
        public ISecurityObject Find(Guid id) 
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return _find(Repo, id); 
        }
        public SecurityObjectKind Kind { get; private set; }

        public RepositorySecObjectProvider( SecurityObjectKind kind ) 
            //: this( kind, ( r, id ) => r.All.FirstOrDefault( e => e.SID == id ) ) 
        {
        }

        //public RepositorySecObjectProvider( SecurityObjectKind kind, Func<IRepository<TDomain, TEntity>, Guid, TEntity> find )
        //{
        //    _find = find;
        //    Kind = kind;
        //}
    }
}