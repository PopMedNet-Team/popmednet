using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Composition;
using Lpp.Mvc;
using Lpp.Security.UI.Models;
using Lpp.Utilities.Legacy;

namespace Lpp.Security.UI
{
    class AccessControlList : IAccessControlList
    {
        [Export, ExportMetadata( ExportScope.Key, TransactionScope.Id )]
        public static IUIWidgetFactory<IAccessControlList> Factory { get { return UIWidget.Factory<IAccessControlList>( html => new AccessControlList( html ) ); } }

        public HtmlHelper Html { get; private set; }
        public AccessControlList( HtmlHelper html ) { Html = html; }

        public IHtmlString With( AclEditModel model, string fieldName )
        {
            return Html.Partial<Views.AccessControlList.Acl>().WithModel(
                new AccessControlListModel
                {
                    Parameters = new AccessControlListParameters { Html = Html, FieldName = fieldName, SubjectSelector = model.SubjectSelector( Html ) },
                    InitialEntries = (from t in model.Entries
                                      let target = t.Key
                                      from e in t.Where( FilterNullSubjects )
                                      group new { target, e } by e.Entry.Subject into subj
                                      let es = subj.Select( e => new { tp = TargetPrivilege.Pair( e.target, e.e.Entry.Privilege.SID, null ), e = e.e } )
                                      select new PrivilegesListModel
                                      {
                                          Subject = subj.Key,
                                          OwnEntries = es.Where( e => e.e.InheritedFrom == null ).Select( e => Pair.Create( e.tp, e.e.Entry.Kind ) ),
                                          InheritedEntries = es.Where( e => e.e.InheritedFrom != null ).Select( e =>
                                              Pair.Create( e.tp, new InheritedPrivilegeModel { Kind = e.e.Entry.Kind, InheritedFrom = e.e.InheritedFrom } ) )
                                      }
                                     ).ToList(),
                    PrivilegeEditors = model.PrivilegeEditors
                } );
        }

        private static bool FilterNullSubjects( AnnotatedAclEntry t )
        {
            if ( t.Entry.Subject == null )
            {
                Trace.Fail( "An ACL entry with undefined subject found. Time for ACL cleanup?" );
                return false;
            }

            return true;
        }
    }

    public struct AccessControlListParameters
    {
        internal HtmlHelper Html { get; set; }
        internal string FieldName { get; set; }
        internal IJsControlledView SubjectSelector { get; set; }
    }
}