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
using System.Diagnostics.Contracts;
using Lpp.Utilities.Legacy;

namespace Lpp.Security.UI
{
    public interface ISecurityUIService<TDomain>
    {
        /// <summary>
        /// Parses ACL from the format returned by the <see cref="IObjectAcl"/> widget
        /// </summary>
        ILookup<BigTuple<Guid>, AclEntry> ParseAcls( string acl );
    }

    public static class SecurityUIServiceExtensions
    {
        public static IEnumerable<AclEntry> ParseSingleAcl<TDomain>( this ISecurityUIService<TDomain> secui, string acl )
        {
            //Contract.Requires( secui != null );
            return secui.ParseAcls( acl ).SingleOrDefault().EmptyIfNull();
        }

        public static ILookup<BigTuple<Guid>, AclEntry> ReplaceObject( this ILookup<BigTuple<Guid>, AclEntry> acl, Guid replaceWhat, Guid replaceWith )
        {
            //Contract.Requires( acl != null );
            //Contract.Ensures( //Contract.Result<ILookup<BigTuple<Guid>, AclEntry>>() != null );

            return acl.ReplaceObject( g => g == replaceWhat ? replaceWith : g );
        }

        public static ILookup<BigTuple<Guid>, AclEntry> ReplaceObject( this ILookup<BigTuple<Guid>, AclEntry> acl, Func<Guid,Guid> replacement )
        {
            //Contract.Requires( acl != null );
            //Contract.Requires( replacement != null );
            //Contract.Ensures( //Contract.Result<ILookup<BigTuple<Guid>, AclEntry>>() != null );

            return (from t in acl
                    from e in t
                    select new { t = BigTuple.Create( t.Key.AsEnumerable().Select( replacement ) ), e }
                   )
                   .ToLookup( x => x.t, x => x.e );
        }
    }
}