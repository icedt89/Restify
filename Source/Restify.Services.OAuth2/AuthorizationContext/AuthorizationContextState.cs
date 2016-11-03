namespace JanHafner.Restify.Services.OAuth2.AuthorizationContext
{
    using System;
    using Header;
    using JetBrains.Annotations;
    using Handler;
    using Storage;

    /// <summary>
    /// Defines the base class for all states an <see cref="IAuthorizationContext"/> can have.
    /// </summary>
    public abstract class AuthorizationContextState
    {
        /// <summary>
        /// The <see cref="IAuthorizationStore"/>.
        /// </summary>
        [NotNull]
        protected readonly IAuthorizationStore AuthorizationStore;

        /// <summary>
        /// The <see cref="IAuthorizationHandler"/>.
        /// </summary>
        [NotNull]
        protected readonly IAuthorizationHandler AuthorizationHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationContextState"/> class.
        /// </summary>
        /// <param name="authorizationStore">The <see cref="IAuthorizationStore"/>.</param>
        /// <param name="authorizationHandler">The <see cref="IAuthorizationHandler"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="authorizationHandler"/>' and '<paramref name="authorizationStore"/>' cannot be null. </exception>
        protected AuthorizationContextState([NotNull] IAuthorizationStore authorizationStore, [NotNull] IAuthorizationHandler authorizationHandler)
        {
            if (authorizationStore == null)
            {
                throw new ArgumentNullException(nameof(authorizationStore));
            }

            if (authorizationHandler == null)
            {
                throw new ArgumentNullException(nameof(authorizationHandler));
            }

            this.AuthorizationStore = authorizationStore;
            this.AuthorizationHandler = authorizationHandler;
        }

        /// <summary>
        /// Starts the authorization process.
        /// </summary>
        [NotNull]
        public abstract AuthorizationState StartAuthorization();

        /// <summary>
        /// Revokes the authorization and clears also the <see cref="IAuthorizationStore"/>.
        /// </summary>
        public abstract void RevokeAuthorization();

        /// <summary>
        /// Creates a Bearer-authorization header.
        /// </summary>
        /// <returns>The Bearer-authorization header.</returns>
        [NotNull]
        public abstract HttpHeader CreateBearer();

        /// <summary>
        /// Refreshes the authorization saved by the state.
        /// </summary>
        public abstract void RefreshAuthorization();
    }
}