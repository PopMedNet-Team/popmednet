using System;
using System.Web.Mvc;
using Lpp.Mvc;

namespace Lpp.Auth.UI
{
    public interface IAuthProviderDefinition
    {
        Guid Guid { get; }
        string Name { get; }
        bool RequireUsername { get; }
        Func<UrlHelper, string> ImageUrl { get; }
    }

    public interface IAuthProviderHandler
    {
        ActionResult PrepareAuthRequest( IAuthProviderDefinition def, string username, Uri returnTo );
        IAuthResult GetResult( IAuthProviderDefinition def );
    }

    public interface IAuthResult
    {
        string UserId { get; }
    }

    public interface IConfigAuthRequest<TRequest>
    {
        void Config( TRequest r );
    }

    public class BaseAuthProvider : IAuthProviderDefinition
    {
        public Guid Guid { get; private set; }
        public string Name { get; private set; }
        public Func<UrlHelper, string> ImageUrl { get; private set; }
        public bool RequireUsername { get; private set; }

        protected BaseAuthProvider( string guid, string name, string imageUrl, bool requireUsername = false )
        {
            Guid = new System.Guid( guid );
            Name = name;
            RequireUsername = requireUsername;
            ImageUrl = url => url.Resource( this.GetType().Assembly, "openid/" + imageUrl );
        }
    }
}