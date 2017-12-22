using System.Diagnostics.Contracts;
using System.Web;
using System.Web.Mvc;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc.Controls
{
    public static class GroupBoxesExtensions
    {
        /// <summary>
        /// Returns a partial html string of the form "class="..." buttons-bindings="..." style="display: none"".
        /// This can inserted into any html element that represents a groupbox as attributes and will create buttons.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="title"></param>
        /// <param name="cssClass"></param>
        /// <param name="maximizable"></param>
        /// <param name="minimizable"></param>
        /// <param name="settingsKey"></param>
        /// <param name="buttonsBindings">Format: cssClass: Knockout click binding, cssClass: knockout click binding, ...</param>
        /// <returns></returns>
        public static IHtmlString GroupBox( this HtmlHelper html,
            string cssClass = null, string buttonsBindings = null )
        {
            //Contract.Requires(html != null);
            //Contract.Ensures(//Contract.Result<IHtmlString>() != null);

            return new MvcHtmlString( string.Format( 
                @"class=""{0}ui-groupbox""{1}",
                cssClass.NullOrEmpty() ? "" : cssClass + " ",
                buttonsBindings.NullOrEmpty() ? "" : " buttons-bindings=\"" + buttonsBindings + "\""
            ) );
        }
    }
}