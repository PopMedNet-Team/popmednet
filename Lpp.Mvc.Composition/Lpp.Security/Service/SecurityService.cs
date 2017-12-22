using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
//using Lpp.Data;
using Lpp.Composition;
using System.Collections;
using Lpp.Utilities.Legacy;

namespace Lpp.Security
{
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    class SecurityService<TDomain> : ISecurityService<TDomain>
    {
        //[Import] public IRepository<TDomain, Data.SecurityTarget> Targets { get; set; }
        //[Import] public IRepository<TDomain, Data.AclEntry> Entries { get; set; }
        [Import] public ICompositionService Composition { get; set; }
        [ImportMany] public IEnumerable<SecurityPrivilege> Privileges { get; set; }
        [ImportMany] public IEnumerable<SecurityTargetKind> AllTargetKinds { get; set; }
        [ImportMany] public IEnumerable<ISecuritySubjectProvider<TDomain>> SubjectProviders { get; set; }
        [ImportMany] public IEnumerable<ISecurityObjectProvider<TDomain>> ObjectProviders { get; set; }

        private readonly Lazy<ILookup<SecurityObjectKind, ISecurityObjectProvider<TDomain>>> _objectProvidersByKind;
        private readonly Hashtable _targets = new Hashtable();
        private readonly LocalThreadMemoizer _memoizer = new LocalThreadMemoizer();

        public IDictionary<Guid, SecurityPrivilege> AllPrivileges { get { return _allPrivileges.Value; } }
        private readonly Lazy<Dictionary<Guid, SecurityPrivilege>> _allPrivileges;

        public SecurityService()
        {
            _objectProvidersByKind = new Lazy<ILookup<SecurityObjectKind,ISecurityObjectProvider<TDomain>>>( () =>
                ObjectProviders.ToLookup( o => o.Kind ) );
            _allPrivileges = Lazy.Value( () => Privileges.ToDictionary( p => p.SID ) );
        }

        public IQueryable<BigTuple<Guid>> AllGrantedTargets( ISecuritySubject subject, Expression<Func<Guid, bool>> privilegeFilter, int arity )
        {
            var q = from e in AllPossibleTargetsWithAcls( arity )
                    where e.Allow && e.SubjectId == subject.ID && privilegeFilter.Invoke( e.PrivilegeId )
                    select e.TargetId;
            return q.Expand();
        }

        public void SetAcl( SecurityTarget target, IEnumerable<AclEntry> entries )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var arity = target.Elements.Count();
            //var id = new Data.SecurityTargetId( target.Id().AsEnumerable() );
            //var tr = _targets[ Tuple.Create( arity, id ) ] as Data.SecurityTarget;
            //if ( tr == null )
            //{
            //    var targetExpr = 
            //        Expr.Create( ( Data.SecurityTarget t ) => t.Arity == arity )
            //        .Compose(
            //            BigTuple<Guid>.MemberAccess.Select( x => 
            //                Expr.Create( ( Data.SecurityTarget t, bool acc ) => acc && x.Invoke( t.ObjectIds ) == x.Invoke( id ) ) )
            //        )
            //        .Expand();
            //    tr = Targets.All.FirstOrDefault( targetExpr );

            //    if ( tr == null )
            //    {
            //        if ( !entries.Any() ) return;
            //        tr = Targets.Add( new Data.SecurityTarget { ObjectIds = new Data.SecurityTargetId( target.Elements.Select( e => e.SID ) ), Arity = arity } );
            //    }

            //    _targets[Tuple.Create( arity, id )] = tr;
            //}

            //var incomingEntries = 
            //    entries.Where( e => e.Kind == AclEntryKind.Deny ).Concat( // Deny should take precedence over Allow, but we shouldn't change the order otherwise
            //    entries.Where( e => e.Kind == AclEntryKind.Allow ) )
            //    .Select( ( e, idx ) => new { p = e.Privilege.SID, s = e.Subject.SID, o = idx, a = e.Kind == AclEntryKind.Allow } )
            //    .ToList();
            //var existingEntries = tr.AclEntries.Select( e => new { p = e.PrivilegeId, s = e.SubjectId, o = e.Order, a = e.Allow } ).ToList();

            //var keepEntries = incomingEntries.Intersect( existingEntries ).ToList();
            //var newEntries = incomingEntries.Except( keepEntries ).ToList();
            //var removeEntries = existingEntries.Except( keepEntries ).ToList();

            //(from x in removeEntries
            // join e in tr.AclEntries on x equals new { p = e.PrivilegeId, s = e.SubjectId, o = e.Order, a = e.Allow }
            // select e)
            // .ForEach( Entries.Remove );

            //newEntries.ForEach( x => Entries.Add( new Data.AclEntry { Target = tr, Order = x.o, PrivilegeId = x.p, SubjectId = x.s, Allow = x.a } ) );
        }

        public IQueryable<SecurityTargetAcl> GetAllAcls( int arity )
        {
            return from t in AllPossibleTargetsWithAcls( arity )
                   group t by t.TargetId into ts
                   select new SecurityTargetAcl
                   {
                       Entries = ts.Select( t => new UnresolvedAclEntry
                       {
                           Allow = t.Allow,
                           PrivilegeId = t.PrivilegeId,
                           SubjectId = t.SubjectId,
                           SourceTarget = t.SourceTargetId,
                           IsInherited = t.IsInherited,
                           ViaMembership = t.ViaMembership,
                           ExplicitAllow = t.ExplicitAllow && !t.ExplicitDeny
                       } ),
                       TargetId = ts.Key
                   };
        }

        public IEnumerable<SecurityTargetKind> KindsFor( SecurityTarget target )
        {
            var kinds = target.Elements.Select( o => o.Kind );
            return AllTargetKinds.Where( k => k.ObjectKindsInOrder.SequenceEqual( kinds ) );
        }

        public IEnumerable<SecurityPrivilege> PrivilegesFor( SecurityTargetKind targetKind )
        {
            return Privileges.Where( p => p.AppliesTo( targetKind ) );
        }

        public SecurityTarget ResolveTarget( BigTuple<Guid> id, SecurityTargetKind kind )
        {
            if ( id.IsEmpty ) return null;

            var providers = kind == null ? null : _objectProvidersByKind.Value;
            var kinds = kind == null ? EnumerableEx.Return<SecurityObjectKind>( null ).Repeat() : kind.ObjectKindsInOrder;

            return new SecurityTarget( 
                id.AsEnumerable()
                .Zip( kinds, ( i, k ) => _memoizer.Memoize( new { objectId = i }, _ =>
                    (k == null ? ObjectProviders : providers[k])
                        .EmptyIfNull()
                        .Select( p => p.Find( i ) )
                        .Where( obj => obj != null )
                        .FirstOrDefault() ) )
                .ToList() );
        }

        public AnnotatedAclEntry ResolveAclEntry( UnresolvedAclEntry e, SecurityTargetKind targetKind )
        {
            return new AnnotatedAclEntry
            {
                Entry = new AclEntry
                {
                    Privilege = Privileges.FirstOrDefault( p => p.SID == e.PrivilegeId ),
                    Subject = _memoizer.Memoize( new { e.SubjectId }, sid => SubjectProviders.Select( p => p.Find( sid.SubjectId ) ).FirstOrDefault( s => s != null ) ),
                    Kind = e.Allow ? AclEntryKind.Allow : AclEntryKind.Deny
                },
                InheritedFrom = e.IsInherited ? ResolveTarget( e.SourceTarget, targetKind ) : null
            };
        }
        
        public IQueryable<TargetWithAclEntry> AllPossibleTargetsWithAcls( int arity )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return Data.Tuples.Tuples.GetAllTuplesWithAcls<TDomain>( Composition, arity );
        }
    }
}