namespace Lpp.Dns.Portal
{
    using System;

    public interface IAuthenticationService
    {
        /// <summary>
        /// Gets the current user.
        /// </summary>
        Lpp.Dns.Data.User CurrentUser { get; }

        /// <summary>
        /// Gets the current ApiIdentity.
        /// </summary>
        Lpp.Utilities.Security.ApiIdentity ApiIdentity { get; }

        /// <summary>
        /// Gets the current user ID.
        /// </summary>
        Guid CurrentUserID { get; }

        /// <summary>
        /// Sets the current user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="scope">The authentication scope</param>
        void SetCurrentUser( Lpp.Dns.Data.User user, AuthenticationScope scope );
    }

    public enum AuthenticationScope
    {
        Transaction,
        WebSession,
        Permanent
    }
}