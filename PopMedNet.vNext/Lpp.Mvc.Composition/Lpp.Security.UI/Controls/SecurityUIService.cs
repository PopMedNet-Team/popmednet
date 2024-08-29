using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Composition.Modules;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using Lpp.Mvc;
using System.Web;
using System.Web.Mvc;
using Lpp.Security.UI.Models;
using Lpp.Composition;
using Lpp.Utilities.Legacy;

namespace Lpp.Security.UI
{
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    class SecurityUIService<TDomain> : ISecurityUIService<TDomain>
    {
        [ImportMany] public IEnumerable<ISecuritySubjectProvider<TDomain>> SubjectProviders { get; set; }
        [ImportMany] public IEnumerable<SecurityPrivilege> Privileges { get; set; }

        public ILookup<BigTuple<Guid>, AclEntry> ParseAcls( string acl )
        {
            var res = from sentry in (acl??"").Split( ',' )
                      let parts = sentry.Split( ':' )
                      where parts.Length == 4

                      let entry = from subjId in Maybe.ParseGuid( parts[0] )
                                  from targetId in ParseTargetId( parts[1] )
                                  from privId in Maybe.ParseGuid( parts[2] )

                                  let skind = parts[3]
                                  from kind in
                                      string.Equals( skind, "allow", StringComparison.InvariantCultureIgnoreCase ) ? Maybe.Value( AclEntryKind.Allow ) :
                                      string.Equals( skind, "deny", StringComparison.InvariantCultureIgnoreCase ) ? Maybe.Value( AclEntryKind.Deny ) :
                                      Maybe.Null<AclEntryKind>()

                                  from subj in SubjectProviders.Select( p => p.Find( subjId ) ).FirstOrDefault( s => s != null )
                                  from priv in Privileges.FirstOrDefault( p => p.SID == privId )
                                  select new { targetId, e = new AclEntry { Subject = subj, Privilege = priv, Kind = kind } }

                      where entry.Kind == MaybeKind.Value
                      select entry.Value;

            return res.ToLookup( x => x.targetId, x => x.e );
        }

        static MaybeNotNull<BigTuple<Guid>> ParseTargetId( string id )
        {
            var parts = id.Split( 'x' );
            var objIds = 
                parts
                .Select( Maybe.ParseGuid )
                .Aggregate(
                    Maybe.Value( Enumerable.Empty<Guid>() ),
                    ( prevElements, objId ) => from es in prevElements
                                               from i in objId
                                               select es.Concat( new[] { i } )
                );

            return from ids in objIds
                   select new BigTuple<Guid>( ids );
        }
    }
}