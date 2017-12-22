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
    public class RepositorySecSubjectProvider<TDomain, TEntity> : ISecuritySubjectProvider<TDomain>
        where TEntity : class, ISecuritySubject
    {
        //[Import] 
        //public IRepository<TDomain,TEntity> Repo { get; set; }
        //private readonly Func<IRepository<TDomain, TEntity>, Guid, TEntity> _find;

        //public RepositorySecSubjectProvider( Func<IRepository<TDomain,TEntity>,Guid,TEntity> find ) 
        //{ 
        //    _find = find; 
        //}

        public RepositorySecSubjectProvider()
        {
        }

        public ISecuritySubject Find( Guid id ) 
        {
            return null;
        }
    }
}