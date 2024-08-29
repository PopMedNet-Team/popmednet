using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Composition.Modules;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
//using Lpp.Data.Composition;
using System.Diagnostics.Contracts;

namespace Lpp.Security
{
    public static class SecurityExtensions
    {
        public static bool AppliesTo<TDomain>( this ISecurityService<TDomain> conf, SecurityPrivilege priv, SecurityTarget target )
        {
            //Contract.Requires( conf != null );
            //Contract.Requires( priv != null );
            //Contract.Requires( target != null );
            return conf
                .KindsFor( target )
                .SelectMany( k => k.ApplicablePrivilegeSets )
                .Any( priv.BelongsTo );
        }

        public static bool AppliesTo( this SecurityPrivilege priv, SecurityTargetKind targetKind )
        {
            //Contract.Requires( priv != null );
            return targetKind == null ? false :
                targetKind.ApplicablePrivilegeSets.Any( priv.BelongsTo );
        }

        public static BigTuple<Guid> Id( this SecurityTarget t )
        {
            return t == null ? new BigTuple<Guid>() : new BigTuple<Guid>( t.Elements.Select( o => o.ID ) );
        }

        public static IEnumerable<UnresolvedAclEntry> AsUnresolved( this IEnumerable<AnnotatedAclEntry> es )
        {
            //Contract.Requires( es != null );
            //Contract.Ensures( //Contract.Result<IEnumerable<UnresolvedAclEntry>>() != null );

            return es.Select( ( e, i ) => new UnresolvedAclEntry
            {
                Allow = e.Entry.Kind == AclEntryKind.Allow,
                IsInherited = e.InheritedFrom != null,
                SourceTarget = e.InheritedFrom.Id(),
                PrivilegeId = e.Entry.Privilege.SID,
                SubjectId = e.Entry.Subject.ID
            } );
        }
    }
}