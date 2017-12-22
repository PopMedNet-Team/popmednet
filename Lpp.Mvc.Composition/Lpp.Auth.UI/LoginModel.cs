using System.Linq.Expressions;
using System;
using Lpp.Mvc.Application;
using Lpp.Data;
using Lpp.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web;

namespace Lpp.Auth.UI
{
    public class LoginModel
    {
        public IEnumerable<IAuthProviderDefinition> Providers { get; set; }
        public string ReturnTo { get; set; }
        public Func<HtmlHelper, IHtmlString> LoginView { get; set; }
    }
}