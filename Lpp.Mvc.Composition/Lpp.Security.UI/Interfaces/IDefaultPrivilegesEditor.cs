using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc;

namespace Lpp.Security.UI
{
    public class DefaultPrivilegesEditor : IJsControlledView
    {
        private readonly IEnumerable<TargetPrivilegePair> _allPrivileges;

        public DefaultPrivilegesEditor( IEnumerable<TargetPrivilegePair> allPrivileges, Func<HtmlHelper, TargetPrivilegePair, IHtmlString> renderPrivilege )
            : this( allPrivileges.Select( tp => TargetPrivilege.Pair( tp.TargetId, tp.PrivilegeId, html => renderPrivilege( html, tp ) ) ) )
        {
            //Contract.Requires( allPrivileges != null );
            //Contract.Requires( renderPrivilege != null );
        }

        public DefaultPrivilegesEditor( IEnumerable<TargetPrivilegePair> allPrivileges )
        {
            //Contract.Requires( allPrivileges != null );
            _allPrivileges = allPrivileges;
        }

        public IHtmlString Render( HtmlHelper html, string handle )
        {
            return html.Partial<Views.AccessControlList.DefaultPrivilegesEditor>().WithModel( new DefaultPrivilegesEditorModel
            {
                Handle = handle,
                AllPrivileges = _allPrivileges
            } );
        }
    }

    public class DefaultPrivilegesEditorModel
    {
        public string Handle { get; set; }
        public IEnumerable<TargetPrivilegePair> AllPrivileges { get; set; }
    }

    public struct TargetPrivilegePair : IComparable<TargetPrivilegePair>
    {
        public Func<HtmlHelper,IHtmlString> View { get; set; }
        public BigTuple<Guid> TargetId { get; set; }
        public Guid PrivilegeId { get; set; }
        public string SerializedId { get { return string.Join( "x", TargetId.AsEnumerable().TakeWhile( x => x != Guid.Empty ) ) + ":" + PrivilegeId; } }

        public int CompareTo( TargetPrivilegePair other )
        {
            var res = TargetId.CompareTo( other.TargetId );
            if ( res == 0 ) res = PrivilegeId.CompareTo( other.PrivilegeId );
            return res;
        }

        public override bool Equals( object obj ) { return obj is TargetPrivilegePair && this.CompareTo( (TargetPrivilegePair)obj ) == 0; }
        public static bool operator==( TargetPrivilegePair a, TargetPrivilegePair b ) { return a.CompareTo( b ) == 0; }
        public static bool operator!=( TargetPrivilegePair a, TargetPrivilegePair b ) { return a.CompareTo( b ) != 0; }
        public override int GetHashCode() { return PrivilegeId.GetHashCode(); }
    }

    public static class TargetPrivilege
    {
        public static TargetPrivilegePair Pair( BigTuple<Guid> targetId, Guid privilegeId, Func<HtmlHelper, IHtmlString> view )
        {
            return new TargetPrivilegePair { TargetId = targetId, PrivilegeId = privilegeId, View = view };
        }

        public static TargetPrivilegePair Pair( SecurityTarget target, SecurityPrivilege privilege, Func<HtmlHelper, IHtmlString> view )
        {
            return new TargetPrivilegePair { TargetId = target.Id(), PrivilegeId = privilege.SID, View = view };
        }
    }

    public static class DefaultPrivilegesEditorExtensions
    {
        public static IJsControlledView DefaultPrivilegesEditorFor<TDomain>( this ISecurityService<TDomain> sec, SecurityTarget target )
        {
            //Contract.Requires( sec != null );
            //Contract.Requires( target != null );
            //Contract.Ensures( //Contract.Result<IJsControlledView>() != null );

            return sec.DefaultPrivilegesEditorFor( target, (_, p) => new MvcHtmlString( p.Name ) );
        }

        public static IJsControlledView DefaultPrivilegesEditorFor<TDomain>( this ISecurityService<TDomain> sec,
            SecurityTarget target, Func<HtmlHelper, SecurityPrivilege, IHtmlString> renderPrivilege )
        {
            return sec.DefaultPrivilegesEditorFor( new[] { target }, (html,_,p) => renderPrivilege( html, p ) );
        }

        public static IEnumerable<TargetPrivilegePair> GetApplicablePrivilegesForEditor<TDomain>( this ISecurityService<TDomain> sec,
            SecurityTarget target, Func<HtmlHelper, SecurityPrivilege, IHtmlString> renderPrivilege = null )
        {
            renderPrivilege = renderPrivilege ?? (( _, p ) => new MvcHtmlString( p.Name ));
            return sec.GetApplicablePrivilegesForEditor( new[] { target }, ( html, _, p ) => renderPrivilege( html, p ) );
        }

        public static IEnumerable<TargetPrivilegePair> GetApplicablePrivilegesForEditor<TDomain>( this ISecurityService<TDomain> sec,
            IEnumerable<SecurityTarget> targets, Func<HtmlHelper, SecurityTarget, SecurityPrivilege, IHtmlString> renderPrivilege = null )
        {
            //Contract.Requires( sec != null );
            //Contract.Requires( targets != null && targets.Any() );
            //Contract.Ensures( //Contract.Result<IEnumerable<TargetPrivilegePair>>() != null );

            renderPrivilege = renderPrivilege ?? (( _, __, p ) => new MvcHtmlString( p.Name ));
            var allKinds = targets.SelectMany( t => sec.KindsFor( t ).Select( k => new { t, tid = t.Id(), k } ) );
            return from p in sec.AllPrivileges.Values
                   from tk in allKinds
                   where tk.k.ApplicablePrivilegeSets.Any( p.BelongsTo )
                   select TargetPrivilege.Pair( tk.tid, p.SID, html => renderPrivilege( html, tk.t, p ) );
        }

        public static IJsControlledView DefaultPrivilegesEditorFor<TDomain>( this ISecurityService<TDomain> sec, 
            IEnumerable<SecurityTarget> targets, Func<HtmlHelper, SecurityTarget, SecurityPrivilege, IHtmlString> renderPrivilege )
        {
            return new DefaultPrivilegesEditor( sec.GetApplicablePrivilegesForEditor( targets, renderPrivilege ) );
        }
    }}