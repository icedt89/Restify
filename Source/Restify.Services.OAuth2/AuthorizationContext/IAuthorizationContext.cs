namespace JanHafner.Restify.Services.OAuth2.AuthorizationContext
{
    using System;
    using Header;
    using JetBrains.Annotations;
    using Request;

    /// <summary>
    /// Defines a context that is bound to an access token and can be shared by multiple services.
    /// It implements <see cref="IDisposable"/> so that Unit-of-Work can simply be realized.
    /// </summary>
    public interface IAuthorizationContext : IDisposable
    {
        /// <summary>
        /// Indicates if the 
        /// </summary>
        Boolean IsAuthorized { get; }

        /// <summary>
        /// Starts the authorization process.
        /// </summary>
        void StartAuthorization();

        /// <summary>
        /// Ensures that all preconditions are met for sending the request.
        /// </summary>
        /// <param name="request">The authorizable request.</param>
        void EnsureAuthorization([NotNull] RequestBase request);

        /// <summary>
        /// Revokes the whole authorization and clears all data associated with it.
        /// </summary>
        void RevokeAuthorization();

        /// <summary>
        /// Clears the authorization information from this context.
        /// </summary>
        void ClearAuthorization();

        /// <summary>
        /// Creates the Bearer-authorization header for the current authorization.
        /// </summary>
        /// <returns>A new <see cref="HttpHeader"/> with the access token.</returns>
        [NotNull]
        HttpHeader CreateBearer();

        /// <summary>
        /// Refreshes the authorization of this <see cref="IAuthorizationContext"/>.
        /// </summary>
        void RefreshAuthorization();

        /// <summary>
        /// Creates a new service that is bound to this <see cref="IAuthorizationContext"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to create.</typeparam>
        /// <returns>An instance of the service.</returns>
        [NotNull]
        TService CreateAuthorizationBoundService<TService>()
            where TService : IRestService;
    }
}