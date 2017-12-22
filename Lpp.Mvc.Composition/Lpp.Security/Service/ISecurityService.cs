using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Lpp.Security
{
    [ContractClass( typeof( Contracts.ISecurityServiceContract<> ) )]
    public interface ISecurityService<TDomain>
    {
        /// <summary>
        /// Returns a list of all Security Object tuples (aka "Security Targets") of the given arity
        /// to which the given subject has been granted the given privilege
        /// </summary>
        IQueryable<BigTuple<Guid>> AllGrantedTargets( ISecuritySubject subject, Expression<Func<Guid, bool>> privilegeFilter, int arity );

        void SetAcl( SecurityTarget target, IEnumerable<AclEntry> entries );
        IQueryable<SecurityTargetAcl> GetAllAcls( int arity );

        IEnumerable<SecurityTargetKind> KindsFor( SecurityTarget target );
        IEnumerable<SecurityPrivilege> PrivilegesFor( SecurityTargetKind targetKind );

        SecurityTarget ResolveTarget( BigTuple<Guid> id, SecurityTargetKind kind );
        AnnotatedAclEntry ResolveAclEntry( UnresolvedAclEntry e, SecurityTargetKind targetKind );

        IDictionary<Guid, SecurityPrivilege> AllPrivileges { get; }
        IEnumerable<SecurityTargetKind> AllTargetKinds { get; }
    }

    public class SecurityTargetAcl
    {
        public BigTuple<Guid> TargetId { get; set; }
        public IEnumerable<UnresolvedAclEntry> Entries { get; set; }
    }

    namespace Contracts
    {
        [ContractClassFor( typeof( ISecurityService<> ) )]
        abstract class ISecurityServiceContract<TDomain> : ISecurityService<TDomain>
        {
            public IQueryable<BigTuple<Guid>> AllGrantedTargets( ISecuritySubject subject, Expression<Func<Guid, bool>> privilegeFilter, int arity )
            {
                //Contract.Requires( subject != null );
                //Contract.Requires( privilegeFilter != null );
                //Contract.Ensures( //Contract.Result<IQueryable<BigTuple<Guid>>>() != null );
                throw new NotImplementedException();
            }

            public void SetAcl( SecurityTarget target, IEnumerable<AclEntry> entries )
            {
                //Contract.Requires( target != null );
                //Contract.Requires( entries != null );
                throw new NotImplementedException();
            }

            public IQueryable<SecurityTargetAcl> GetAllAcls( int arity )
            {
                //Contract.Ensures( //Contract.Result<IQueryable<SecurityTargetAcl>>() != null );
                throw new NotImplementedException();
            }

            public IEnumerable<SecurityTargetKind> KindsFor( SecurityTarget target )
            {
                //Contract.Requires( target != null );
                //Contract.Ensures( //Contract.Result<IEnumerable<SecurityTargetKind>>() != null );
                throw new NotImplementedException();
            }

            public IEnumerable<SecurityPrivilege> PrivilegesFor( SecurityTargetKind targetKind )
            {
                //Contract.Requires( targetKind != null );
                //Contract.Ensures( //Contract.Result<IEnumerable<SecurityPrivilege>>() != null );
                throw new NotImplementedException();
            }

            public SecurityTarget ResolveTarget( BigTuple<Guid> id, SecurityTargetKind kind )
            {
                throw new NotImplementedException();
            }

            public AnnotatedAclEntry ResolveAclEntry( UnresolvedAclEntry e, SecurityTargetKind targetKind )
            {
                throw new NotImplementedException();
            }
            
            public IDictionary<Guid, SecurityPrivilege> AllPrivileges
            {
                get { //Contract.Ensures( //Contract.Result<IDictionary<Guid,SecurityPrivilege>>() != null ); 
                    throw new NotImplementedException(); }
            }

            public IEnumerable<SecurityTargetKind> AllTargetKinds
            {
                get { //Contract.Ensures( //Contract.Result<IEnumerable<SecurityTargetKind>>() != null ); 
                    throw new NotImplementedException(); }
            }
        }
    }
}