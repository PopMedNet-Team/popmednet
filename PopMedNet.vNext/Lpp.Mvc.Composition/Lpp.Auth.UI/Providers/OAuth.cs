using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using Lpp.Composition;

namespace Lpp.Auth.UI
{
    public static partial class AuthProviders
    {
        public static IAuthProviderDefinition Facebook( string appId, string appSecret )
        {
            return new OAuthProvider( "{D8AEC9E8-9146-4789-86B4-8CC1AFFF3024}", "Facebook", "facebook.jpg",
                "https://graph.facebook.com/oauth/access_token", "https://graph.facebook.com/oauth/authorize",
                appId, ClientCredentialApplicator.PostParameter( appSecret ) );
        }
    }

    public class OAuthProvider : BaseAuthProvider
    {
        public AuthorizationServerDescription Description { get; private set; }
        public string ClientIdentifier { get; private set; }
        public ClientCredentialApplicator CredentialApplicator { get; private set; }

        public OAuthProvider( string guid, string name, string imageUrl, 
            string tokenEndpoint, string authorizationEndpoint, string clientIdentifier, ClientCredentialApplicator credentialApplicator,
            bool requireUsername = false )
            : base( guid, name, imageUrl, requireUsername )
        {
            Description = new AuthorizationServerDescription { TokenEndpoint = new Uri( tokenEndpoint ), AuthorizationEndpoint = new Uri( authorizationEndpoint ) };
            ClientIdentifier = clientIdentifier;
            CredentialApplicator = credentialApplicator;
        }
    }

    public class OAuthResult : IAuthResult
    {
        public IAuthorizationState AuthState { get; private set; }
        public string UserId { get { return Convert.ToString( Fields.ValueOrDefault( "link" ) ); } }
        public IDictionary<string, object> Fields { get; private set; }
        public OAuthResult( IAuthorizationState s, IDictionary<string,object> fields ) {
            Contract.Requires( s != null );
            Contract.Requires( fields != null );
            AuthState = s;
            Fields = fields;
        }
    }

    [Export( typeof( IAuthProviderHandler ) ), PartMetadata( ExportScope.Key, TransactionScope.Id )]
    class OAuthProviderHandler : IAuthProviderHandler
    {
        [Import] internal HttpContextBase HttpContext { get; set; }
        [ImportMany] internal IEnumerable<IConfigAuthRequest<WebServerClient>> Configs { get; set; }

        T Do<T>( IAuthProviderDefinition def, Func<WebServerClient, T> f )
        {
            var provider = def as OAuthProvider;
            if ( provider == null ) return default( T );

            var r = new WebServerClient( provider.Description, provider.ClientIdentifier, provider.CredentialApplicator );
            return f( r );
        }

        public ActionResult PrepareAuthRequest( IAuthProviderDefinition def, string username, Uri returnTo )
        {
            return Do( def, r =>
                Configs.EmptyIfNull().Aggregate( r, (rr,c) => { c.Config( rr ); return rr; } )
                .PrepareRequestUserAuthorization( null, returnTo ).AsActionResult()
            );
        }

        public IAuthResult GetResult( IAuthProviderDefinition def )
        {
            return Do( def, r =>
            {
                var s = r.ProcessUserAuthorization( HttpContext.Request );
                if ( s == null ) return null;

                var content = new WebClient().DownloadString( "https://graph.facebook.com/me?access_token=" + s.AccessToken );
                var fields = new JavaScriptSerializer().Deserialize<Dictionary<string,object>>( content );
                return new OAuthResult( s, fields );
            } );
        }
    }
}