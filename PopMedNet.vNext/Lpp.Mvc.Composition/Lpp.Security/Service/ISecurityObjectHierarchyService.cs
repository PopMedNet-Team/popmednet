using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Lpp.Security
{
    [ContractClass( typeof( Contracts.ISecurityObjectHierarchyServiceContract<> ) )]
    public interface ISecurityObjectHierarchyService<TDomain>
    {
        void SetObjectInheritanceParent( ISecurityObject obj, ISecurityObject parent );
        IQueryable<Guid> GetObjectChildren( ISecurityObject obj, bool includeSelf = false );
        IQueryable<Guid> GetObjectTransitiveChildren( ISecurityObject obj, bool includeSelf = false );
        Expression<Func<Guid, IQueryable<Guid>>> GetObjectChildren( bool includeSelf = false );
        Expression<Func<Guid, IQueryable<Guid>>> GetObjectTransitiveChildren( bool includeSelf = false );
    }

    namespace Contracts
    {
        [ContractClassFor( typeof( ISecurityObjectHierarchyService<> ) )]
        abstract class ISecurityObjectHierarchyServiceContract<TDomain> : ISecurityObjectHierarchyService<TDomain>
        {
            public void SetObjectInheritanceParent( ISecurityObject obj, ISecurityObject parent )
            {
                //Contract.Requires( obj != null );
                throw new NotImplementedException();
            }

            public IQueryable<Guid> GetObjectChildren( ISecurityObject obj, bool includeSelf = false )
            {
                //Contract.Requires( obj != null );
                //Contract.Ensures( //Contract.Result<IQueryable<Guid>>() != null );
                return null;
            }

            public IQueryable<Guid> GetObjectTransitiveChildren( ISecurityObject obj, bool includeSelf = false )
            {
                //Contract.Requires( obj != null );
                //Contract.Ensures( //Contract.Result<IQueryable<Guid>>() != null );
                return null;
            }

            public Expression<Func<Guid, IQueryable<Guid>>> GetObjectChildren( bool includeSelf = false )
            {
                //Contract.Ensures( //Contract.Result<Expression<Func<Guid, IQueryable<Guid>>>>() != null );
                return null;
            }

            public Expression<Func<Guid, IQueryable<Guid>>> GetObjectTransitiveChildren( bool includeSelf = false )
            {
                //Contract.Ensures( //Contract.Result<Expression<Func<Guid, IQueryable<Guid>>>>() != null );
                return null;
            }
        }
    }
}