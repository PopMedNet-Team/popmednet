using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics.Contracts;
using Lpp.Mvc;

namespace Lpp.Mvc.Controls
{
    public interface IMenu : IUIWidget
    {
        IHtmlString FromItems( IEnumerable<MenuItemModel> items );
    }
}