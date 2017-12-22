using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Lpp.Security
{
    [ContractClass( typeof( Contracts.ISecurityMembershipServiceContract<> ) )]
    public interface ISecurityMembershipService<TDomain>
    {
        void SetSubjectParents( ISecuritySubject subj, IEnumerable<ISecuritySubject> memberOf );
        IEnumerable<UnresolvedSubject> GetSubjectParents( ISecuritySubject subj, bool immediateOnly = true );
        IEnumerable<UnresolvedSubject> GetSubjectChildren( ISecuritySubject subj, bool immediateOnly = true );
    }

    public class UnresolvedSubject
    {
        public Guid Id { get; set; }
        public Func<ISecuritySubject> Resolve { get; set; }
    }

    public static class SecurityMembershipExtensions
    {
        public static IEnumerable<ISecuritySubject> Resolve( this IEnumerable<UnresolvedSubject> subjs )
        {
            return subjs.Where( s => s != null ).Select( s => s.Resolve() ).Where( s => s != null );
        }
    }

    namespace Contracts
    {
        [ContractClassFor( typeof( ISecurityMembershipService<> ) )]
        abstract class ISecurityMembershipServiceContract<TDomain> : ISecurityMembershipService<TDomain>
        {
            public void SetSubjectParents( ISecuritySubject subj, IEnumerable<ISecuritySubject> memberOf )
            {
                throw new NotImplementedException();
            }

            public IEnumerable<UnresolvedSubject> GetSubjectParents( ISecuritySubject subj, bool immediateOnly )
            {
                throw new NotImplementedException();
            }

            public IEnumerable<UnresolvedSubject> GetSubjectChildren( ISecuritySubject subj, bool immediateOnly )
            {
                throw new NotImplementedException();
            }
        }
    }
}