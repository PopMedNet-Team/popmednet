using System.Diagnostics.Contracts;
using System.Web;
using System.Web.Mvc;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc.Controls
{
    public static class SectionsExtensions
    {
        /// <summary>
        /// Returns a partial html string of the form "class="..." title-text="..." data-settingsKey="..." buttons-bindings="..." style="display: none"".
        /// This can inserted into any html element that represents a section as attributes and will create title, and minimize, maximize plus other buttons,
        /// and provide a key to save its current state.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="title"></param>
        /// <param name="cssClass"></param>
        /// <param name="maximizable"></param>
        /// <param name="minimizable"></param>
        /// <param name="settingsKey"></param>
        /// <param name="buttonsBindings">Format: cssClass: Knockout click binding, cssClass: knockout click binding, ...</param>
        /// <returns></returns>
        public static IHtmlString Section( this HtmlHelper html,
            string title = null, string cssClass = null,
            bool maximizable = false, bool minimizable = true, string settingsKey = null, string buttonsBindings = null )
        {
            //Contract.Requires(html != null);
            //Contract.Ensures(//Contract.Result<IHtmlString>() != null);

            var s = html.Settings( settingsKey );
            var max = s.Get<bool>( "maximized" );
            var min = s.Get<bool>( "minimized" );
            var minmaxClass =
                max 
                ? min ? " ui-maximized ui-minimized" : " ui-maximized"
                : min ? " ui-minimized" : "";

            return new MvcHtmlString( string.Format( 
                @"class=""{0}ui-section ui-has-header{1}{2}{3}""{4}{5}{6}{7}",
                cssClass.NullOrEmpty() ? "" : cssClass + " ",
                minimizable ? " ui-minimizable" : "",
                maximizable ? " ui-maximizable" : "",
                minmaxClass,
                title.NullOrEmpty() ? "" : " title-text=\"" + title + "\"",
                settingsKey.NullOrEmpty() ? "" : " data-settingsKey=\"" + settingsKey + "\"",
                buttonsBindings.NullOrEmpty() ? "" : " buttons-bindings=\"" + buttonsBindings + "\"",
                min ? " style=\"display: none\"" : ""
            ) );
        }

        public static IHtmlString SectionState( this ClientSettingsHelper s )
        {
            var max = s.Get<bool>( "maximized" );
            var min = s.Get<bool>( "minimized" );
            var res =
                max 
                ? min ? "ui-maximized ui-minimized" : "ui-maximized"
                : min ? "ui-minimized" : "";
            return new MvcHtmlString( res );
        }
    }
}