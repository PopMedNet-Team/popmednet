using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using Lpp.Composition;

namespace Lpp.Auth.UI
{
    public static partial class AuthProviders
    {
        public static readonly IAuthProviderDefinition Google = new OpenIdProvider( "{F28F83F1-D997-4841-8CE5-F5F720EF2BA4}", "http://www.google.com/accounts/o8/id", "Google", "google.jpg" );
        public static readonly IAuthProviderDefinition Yahoo = new OpenIdProvider( "{2CD19C7F-09FA-498E-AAAC-3A5A774BCB2B}", "http://yahoo.com", "Yahoo!", "yahoo.jpg" );
        public static readonly IAuthProviderDefinition LiveJournal = new OpenIdProvider( "{63E3D2E9-2196-4CFE-B3D9-734E28F06584}", "http://{0}.livejournal.com", "LiveJournal", "livejournal.jpg", true );
        public static readonly IAuthProviderDefinition Yandex = new OpenIdProvider( "{D344EFA7-CF46-4284-8F47-242781F8DAFD}", "http://openid.yandex.ru", "Yandex", "yandex.jpg" );

        public static readonly IAuthProviderDefinition[] AllOpenId = new[] { Google, Yahoo, LiveJournal, Yandex };
    }

    public class OpenIdProvider : BaseAuthProvider
    {
        public string Id { get; private set; }

        public OpenIdProvider( string guid, string id, string name, string imageUrl, bool requireUsername = false )
            : base( guid, name, imageUrl, requireUsername )
        {
            Id = id;
        }
    }

    public class OpenIdAuthResult : IAuthResult
    {
        public IAuthenticationResponse Response { get; private set; }
        public string UserId { get { return Response.ClaimedIdentifier; } }
        public FetchResponse Fields { get { return Response.GetExtension<FetchResponse>() ?? new FetchResponse(); } }
        public OpenIdAuthResult( IAuthenticationResponse r ) { Response = r; }
    }

    [Export(typeof( IAuthProviderHandler ) ), PartMetadata( ExportScope.Key, TransactionScope.Id )]
    class OpenIdProviderHandler : IAuthProviderHandler
    {
        private static readonly OpenIdRelyingParty _openid = new OpenIdRelyingParty( new StandardRelyingPartyApplicationStore() );
        [Import] internal HttpContextBase HttpContext { get; set; }
        [ImportMany] internal IEnumerable<IConfigAuthRequest<IAuthenticationRequest>> Configs { get; set; }

        public ActionResult PrepareAuthRequest( IAuthProviderDefinition def, string username, Uri returnTo )
        {
            var provider = def as OpenIdProvider;
            if ( provider == null ) return null;

            var r = _openid.CreateRequest( Identifier.Parse( string.Format( provider.Id, username ) ), Realm.AutoDetect, returnTo );
            foreach( var c in Configs.EmptyIfNull() ) c.Config( r );
            
            return r.RedirectingResponse.AsActionResult();
        }

        public IAuthResult GetResult( IAuthProviderDefinition def )
        {
            if ( !(def is OpenIdProvider) ) return null;

            var r = _openid.GetResponse( HttpContext.Request );
            return r == null || r.Status != AuthenticationStatus.Authenticated ? null : new OpenIdAuthResult( r );
        }
    }
}