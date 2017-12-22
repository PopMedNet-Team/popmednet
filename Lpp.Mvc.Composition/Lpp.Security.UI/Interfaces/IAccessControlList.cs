using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Composition;
using Lpp.Mvc;
using Lpp.Utilities.Legacy;

namespace Lpp.Security.UI
{
    public interface IAccessControlList : IUIWidget
    {
        /// <summary>
        /// Renders a list of current objects's privileges and subjects to whom those privileges are granted,
        /// and allows to edit that list.
        /// </summary>
        /// <param name="target">The target to operate on.</param>
        /// <param name="fieldName">Name of the form field that will hold the resulting ACL. See remarks.</param>
        /// <param name="groupSelector">UI for choosing a group</param>
        /// <remarks>
        /// The format of the resulting ACL is a comma-delimited list of ACL entries, where each entry is:
        /// 
        ///     subject-id ":" target-id ":" privilege-id ":" kind
        ///     where:
        ///         subject-id      = GUID
        ///         target-id       = object-id ("x" object-id)*
        ///         object-id       = GUID
        ///         privilege-id    = GUID
        ///         kind            = "allow" | "deny"
        /// 
        /// Example:
        /// 
        ///     7120FBF8-7664-4F1D-B29C-109ACFA13128:729F23B4-191A-42C7-86AA-B6ED33BFFB9Bx580060A9-271D-4F13-BAAC-17A42F9F3F4C:CC51A8D7-E92F-4779-BF8C-8C54E0FDD8CD:allow
        ///     
        /// To parse this format, use <see cref="ISecurityUIService&lt;TDomain&gt;.ParseAcl"/>
        /// </remarks>
        IHtmlString With( AclEditModel model, string fieldName );
    }

    public class AclEditModel
    {
        public ILookup<BigTuple<Guid>,AnnotatedAclEntry> Entries { get; set; }
        public IEnumerable<IJsControlledView> PrivilegeEditors { get; set; }
        public Func<HtmlHelper, IJsControlledView> SubjectSelector { get; set; }
    }

    public static class AccessControlListExtensions
    {
        public static AclEditModel DefaultAclEditModelFor<TDomain>( this ISecurityService<TDomain> sec, SecurityTarget target, Func<HtmlHelper, IJsControlledView> subjectSelector )
        {
            //Contract.Requires( sec != null );
            //Contract.Requires( target != null );
            //Contract.Ensures( //Contract.Result<AclEditModel>() != null );

            return sec.DefaultAclEditModelFor( new[] { target }, subjectSelector, (_, __, p) => new MvcHtmlString( p.Name ) );
        }

        public static ILookup<BigTuple<Guid>, AnnotatedAclEntry> AllEntriesForEdit<TDomain>( this ISecurityService<TDomain> sec, SecurityTarget target )
        {
            //Contract.Requires( sec != null );
            //Contract.Requires( target != null );
            return sec.AllEntriesForEdit( new[] { target } );
        }

        public static ILookup<BigTuple<Guid>, AnnotatedAclEntry> AllEntriesForEdit<TDomain>( this ISecurityService<TDomain> sec, IEnumerable<SecurityTarget> targets )
        {
            var es = from target in targets
                     let tid = target.Id()
                     let kind = sec.KindsFor( target ).FirstOrDefault()
                     let _ = new Func<int>( () =>
                     {
                         if ( kind == null ) throw new InvalidOperationException( "There is no SecurityTargetKind registered for the tuple of [" +
                            string.Join( ", ", target.Elements.Select( e => e.Kind.Name ) ) + "]. Use Sec.TargetKind() method to create a target kind, then export it into MEF context." );
                         return 0;
                     } )()
                     from e in sec.GetAcl( target ).SkipMembershipImplied().ResolveAcl( sec, kind )
                     select new { e, tid };

            return es.ToLookup( e => e.tid, e => e.e );
        }

        public static AclEditModel DefaultAclEditModelFor<TDomain>( this ISecurityService<TDomain> sec, 
            IEnumerable<SecurityTarget> targets, Func<HtmlHelper,IJsControlledView> subjectSelector, Func<HtmlHelper, SecurityTarget, SecurityPrivilege, IHtmlString> renderPrivilege = null )
        {
            //Contract.Requires( sec != null );
            //Contract.Requires( targets != null && targets.Any() );
            //Contract.Ensures( //Contract.Result<AclEditModel>() != null );

            renderPrivilege = renderPrivilege ?? ((_,__,p) => new MvcHtmlString( p.Name ));
            return new AclEditModel
            {
                Entries = AllEntriesForEdit( sec, targets ),
                SubjectSelector = subjectSelector,
                PrivilegeEditors = new[] { sec.DefaultPrivilegesEditorFor( targets, renderPrivilege ) }
            };
        }

        public static AclEditModel AddPrivilegesEditor( this AclEditModel m, IJsControlledView editor )
        {
            //Contract.Requires( m != null );
            //Contract.Requires( editor != null );
            //Contract.Ensures( //Contract.Result<AclEditModel>() != null );

            return new AclEditModel
            {
                Entries = m.Entries,
                SubjectSelector = m.SubjectSelector,
                PrivilegeEditors = m.PrivilegeEditors.EmptyIfNull().Concat( new[] { editor } )
            };
        }

        public static AclEditModel AddEntries( this AclEditModel m, ILookup<BigTuple<Guid>, AnnotatedAclEntry> entries )
        {
            //Contract.Requires( m != null );
            //Contract.Requires( entries != null );
            //Contract.Ensures( //Contract.Result<AclEditModel>() != null );

            return new AclEditModel
            {
                Entries = m.Entries.Merge( entries ),
                SubjectSelector = m.SubjectSelector,
                PrivilegeEditors = m.PrivilegeEditors
            };
        }

        public static AclEditModel AddTargets<TDomain>( this AclEditModel m, ISecurityService<TDomain> sec, IEnumerable<SecurityTarget> ts )
        {
            //Contract.Requires( m != null );
            //Contract.Requires( ts != null );
            //Contract.Ensures( //Contract.Result<AclEditModel>() != null );
            return m.AddEntries( AllEntriesForEdit( sec, ts ) );
        }

        public static IHtmlString For<TDomain>( this IAccessControlList lst, SecurityTarget target, string fieldName, Func<HtmlHelper, IJsControlledView> subjSelector )
        {
            //Contract.Requires( lst != null );
            //Contract.Requires( target != null );
            //Contract.Requires( !String.IsNullOrEmpty( fieldName ) );
            //Contract.Requires( subjSelector != null );
            //Contract.Ensures( //Contract.Result<IHtmlString>() != null );

            var sec = lst.Html.ViewContext.HttpContext.Composition().Get<ISecurityService<TDomain>>();
            return lst.With( sec.DefaultAclEditModelFor( target, subjSelector ), fieldName );
        }
    }
}