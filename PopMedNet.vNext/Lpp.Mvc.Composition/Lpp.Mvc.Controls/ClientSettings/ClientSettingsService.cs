using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Composition;

namespace Lpp.Mvc.Controls
{
    [Export( typeof( IClientSettingsService ) )]
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    class ClientSettingsService : IClientSettingsService
    {
        [ImportMany] public IEnumerable<Lazy<IClientSettingsProvider>> Providers { get; set; }
        [Import] public HttpContextBase HttpContext { get; set; }

        public string GetSetting( string key )
        {
            return Providers
                .Select( p => p.Value.GetSetting( key ) )
                .Where( s => s != null )
                .FirstOrDefault();
        }

        public void SetSettings( IDictionary<string, string> ss )
        {
            Providers
                .Select( p => p.Value.SetSettings( ss ) )
                .TakeWhile( s => s != null && s.Any() )
                .LastOrDefault();
        }

        public string ClientSetSettingScriptReferenceUrl { get { return new UrlHelper( HttpContext.Request.RequestContext ).Action( ( ClientSettingsController c ) => c.ClientScript() ); } }
    }
}