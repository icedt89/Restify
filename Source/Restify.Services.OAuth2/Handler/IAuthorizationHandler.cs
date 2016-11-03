namespace JanHafner.Restify.Services.OAuth2.Handler
{
    using JetBrains.Annotations;

    /// <summary>
    /// Handles the authorization.
    /// </summary>
    public interface IAuthorizationHandler
    {
        /// <summary>
        /// Begins the authorization process.
        /// </summary>
        [CanBeNull]
        AuthorizationState StartAuthorization();

        /// <summary>
        /// Refreshs the supplied authorization using the refresh token from the <see cref="AuthorizationState"/>.
        /// </summary>
        /// <param name="current">The authorization that should be refreshed.</param>
        /// <returns>The refreshed authorization.</returns>
        [CanBeNull]
        AuthorizationState RefreshAuthorization([NotNull] AuthorizationState current);
    }
}