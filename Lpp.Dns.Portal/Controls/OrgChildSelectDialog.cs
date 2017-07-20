using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc.Application;
using Lpp.Dns.Data;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using Lpp.Composition;
using Lpp.Dns.Portal.Models;
using System.Linq.Expressions;
using Lpp.Dns.Portal.Controllers;

namespace Lpp.Dns.Portal.Controls
{
    public class OrgChildSelectDialog : IUIWidget
    {
        public HtmlHelper Html { get; private set; }
        //private readonly IRepository<DnsDomain, Organization> _organizations;

        //TODO: this will be needed when IRepository is gone, passing something else
        //public OrgChildSelectDialog(HtmlHelper html, IRepository<DnsDomain, Organization> orgs)
        //{
        //    Html = html;
        //    _organizations = orgs;
        //}

        public IHtmlString With(RoutedComputation<RenderOrgChildrenList> children, string chooseFunctionName, string dialogTitle)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return Html.Partial<Views.Organizations.OrgChildSelectDialog>().WithModel(new OrgChildrenSelectorModel
            //{
            //    Children = children,
            //    Organizations = _organizations.All.Where(o => o.Parent == null),
            //    ChooseFunctionName = chooseFunctionName,
            //    DialogTitle = dialogTitle
            //});
        }
    }

    public static class OrgChildrenSelectorExtensions
    {
        public static IHtmlString With<TController>(this OrgChildSelectDialog sel,
            Expression<Func<TController, ComputationResult<RenderOrgChildrenList>>> getChildren,
            string chooseFunctionName, string dialogTitle)
            where TController : Controller
        {
            return sel.With(new UrlHelper(sel.Html.ViewContext.RequestContext).RoutedComputation(getChildren),
                chooseFunctionName, dialogTitle);
        }
    }

    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    [Export(typeof(IUIWidgetFactory<OrgChildSelectDialog>))]
    class OrgChildrenSelectorFactory : IUIWidgetFactory<OrgChildSelectDialog>
    {
        //[Import]
        //public IRepository<DnsDomain, Organization> Organizations { get; set; }

        public OrgChildSelectDialog CreateWidget(HtmlHelper html)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return new OrgChildSelectDialog(html, Organizations);
        }
    }

    public delegate Func<HtmlHelper, IHtmlString> RenderOrgChildrenList(Guid orgId, string searchTerm);
}