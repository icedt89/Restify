namespace JanHafner.Restify.Services.OAuth2.Handler
{
    using JetBrains.Annotations;

    /// <summary>
    /// Does nothing.
    /// </summary>
    public sealed class NullAuthorizationHandler : IAuthorizationHandler
    {
        /// <summary>
        /// Begins the authorization process.
        /// </summary>
        public AuthorizationState StartAuthorization()
        {
            return null;
        }

        /// <summary>
        /// Refreshs the supplied authorization using the refresh token from the <see cref="AuthorizationState"/>.
        /// </summary>
        /// <param name="current">The authorization that should be refreshed.</param>
        /// <returns>The refreshed authorization.</returns>
        public AuthorizationState RefreshAuthorization([CanBeNull] AuthorizationState current)
        {
            return null;
        }
    }
}