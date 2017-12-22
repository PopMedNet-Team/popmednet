using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Lpp.Composition;

namespace Lpp.Security
{
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    class SecurityMembershipService<TDomain> : ISecurityMembershipService<TDomain>
    {
        [Import] public DagService<TDomain, Guid, Data.MembershipEdge, Data.MembershipClosureEdge> Dag { get; set; }
        [ImportMany] public IEnumerable<ISecuritySubjectProvider<TDomain>> Providers { get; set; }

        public void SetSubjectParents( ISecuritySubject subj, IEnumerable<ISecuritySubject> memberOf )
        {
            Dag.SetAdjacency( subj.ID, memberOf.Select( m => m.ID ) );
        }

        public IEnumerable<UnresolvedSubject> GetSubjectParents( ISecuritySubject subj, bool immediateOnly = true )
        {
            return Dag.GetAdjacentEnds( subj.ID, immediateOnly ).Select( CreateSubj );
        }

        public IEnumerable<UnresolvedSubject> GetSubjectChildren( ISecuritySubject subj, bool immediateOnly = true )
        {
            return Dag.GetAdjacentStarts( subj.ID, immediateOnly ).Select( CreateSubj );
        }

        UnresolvedSubject CreateSubj( Guid id )
        {
            return new UnresolvedSubject { Id = id, Resolve = () => Resolve( id ) };
        }

        ISecuritySubject Resolve( Guid id )
        {
            return Providers.Select( p => p.Find( id ) ).Where( f => f != null ).FirstOrDefault();
        }
    }
}