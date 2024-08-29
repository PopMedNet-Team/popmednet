using System;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Audit.UI
{
    public class ClientControlDisplay
    {
        public string ValueAsString { get; set; }

        /// <summary>
        /// The second argument is a Javascript expression that resolves to a function which will act as the onchanged handler
        /// </summary>
        public Func<HtmlHelper, string, IHtmlString> Render { get; set; }
    }
}