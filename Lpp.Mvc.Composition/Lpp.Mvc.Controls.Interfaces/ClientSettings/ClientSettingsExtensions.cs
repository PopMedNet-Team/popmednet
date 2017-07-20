using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Composition;

namespace Lpp.Mvc.Controls
{
    public static class ClientSettingsExtensions
    {
        public static ClientSettingsHelper Settings( this HtmlHelper html, string baseKey ) { return new ClientSettingsHelper( html, baseKey ); }

        public static IHtmlString ClientSettingsScript( this HtmlHelper html )
        {
            return html.Raw( html.ViewContext.HttpContext.Composition().Get<IClientSettingsService>().ClientSetSettingScriptReferenceUrl );
        }
    }

    public struct ClientSettingsHelper
    {
        public HtmlHelper Html { get; private set; }
        private readonly string _baseKey;
        private readonly IClientSettingsService _service;

        public ClientSettingsHelper( HtmlHelper html, string baseKey ) : this()
        {
            Html = html;
            _baseKey = baseKey;
            _service = html.ViewContext.HttpContext.Composition().Get<IClientSettingsService>();
        }

        public string Get( string key ) { return _baseKey == null ? null : _service.GetSetting( _baseKey + "." + key ); }
        public void Set( string key, string value ) { if ( _baseKey != null ) _service.SetSettings( new SortedList<string,string> { { _baseKey + "." + key, value } } ); }

        public T Get<T>( string key )
        {
            try
            {
                var v = Get( key );
                if ( v == null ) return default( T );

                var res = Convert.ChangeType( v, typeof( T ) );
                if ( res == null ) return default( T );
                return (T)res;
            }
            catch ( FormatException )
            {
                return default( T );
            }
        }
    }
}