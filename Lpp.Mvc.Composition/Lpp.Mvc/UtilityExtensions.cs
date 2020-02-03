using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Web.Mvc;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc
{
    public static class UtilityExtensions
    {
        public static string ResourceUrl(this WebViewPage page, string url)
        {
            return string.Format("{0}{1}", System.Web.Configuration.WebConfigurationManager.AppSettings["ResourceUrl"], url);
        }

        public static string UniqueId( this HtmlHelper html )
        {
            //Contract.Ensures( !String.IsNullOrEmpty( //Contract.Result<string>() ) );
            return uniqueid();
        }

        public static string UniqueId( this System.Web.WebPages.Html.HtmlHelper html )
        {
            //Contract.Ensures( !String.IsNullOrEmpty( //Contract.Result<string>() ) );
            return uniqueid();
        }

        public static void Include( this ModelStateDictionary st, OpResult res )
        {
            foreach ( var m in res.ErrorMessages.EmptyIfNull() ) st.AddModelError( "", m );
        }

        public static IEnumerable<T> GetAttributes<T>( this ActionDescriptor ad )
        {
            //Contract.Requires( ad != null );
            //Contract.Ensures( //Contract.Result<IEnumerable<T>>() != null );

            return 
                ad.GetCustomAttributes( typeof( T ), true ).Cast<T>()
                .Concat( 
                ad.ControllerDescriptor.GetCustomAttributes( typeof( T ), true ).Cast<T>() );
        }

        static string uniqueid()
        {
            return "_" + Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 0, 6 );
        }
    }
}