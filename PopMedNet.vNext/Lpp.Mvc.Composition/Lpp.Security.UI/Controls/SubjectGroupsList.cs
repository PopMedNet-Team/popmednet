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

namespace Lpp.Security.UI
{
    class SubjectGroupsList : ISubjectGroupsList
    {
        [Export, ExportMetadata( ExportScope.Key, TransactionScope.Id )]
        public static IUIWidgetFactory<ISubjectGroupsList> Factory { get { return UIWidget.Factory<ISubjectGroupsList>( html => new SubjectGroupsList( html ) ); } }

        public HtmlHelper Html { get; private set; }
        public SubjectGroupsList( HtmlHelper html ) { Html = html; }

        public ISubjectGroupsList<TDomain> ForDomain<TDomain>()
        {
            return new Gen<TDomain> { Html = Html };
        }

        class Gen<TDomain> : ISubjectGroupsList<TDomain>
        {
            public HtmlHelper Html { get; set; }

            public IHtmlString ForSubject( ISecuritySubject subj, string fieldName, IJsControlledView gs, bool enabled = true )
            {
                var model = new SubjectGroupsListModel
                    {
                        FieldName = fieldName,
                        Subject = subj,
                        MemberOf = Html.ViewContext.HttpContext.Composition().Get<ISecurityMembershipService<TDomain>>().GetSubjectParents(subj).Resolve(),
                        GroupSelector = gs
                    };
                return enabled ? Html.Partial<Views.SubjectGroupsSelector.Selector>().WithModel(model) : Html.Partial<Views.SubjectGroupsSelector.SubjectGroups>().WithModel(model);
            }
        }
    }
}