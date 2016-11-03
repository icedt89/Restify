namespace JanHafner.Restify.Services.OAuth2.AuthorizationContext
{
    using System;
    using Handler;
    using Header;
    using Properties;
    using Storage;

    /// <summary>
    /// Defines the state where the <see cref="IAuthorizationContext"/> is unauthorized.
    /// </summary>
    public sealed class UnauthorizedAuthorizationContextState : AuthorizationContextState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationContextState"/> class.
        /// </summary>
        /// <param name="authorizationStore">The <see cref="IAuthorizationStore"/>.</param>
        /// <param name="authorizationHandler">The <see cref="IAuthorizationHandler"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="authorizationHandler" />' and '<paramref name="authorizationStore" />' cannot be null. </exception>
        public UnauthorizedAuthorizationContextState(IAuthorizationStore authorizationStore, IAuthorizationHandler authorizationHandler)
            : base(authorizationStore, authorizationHandler)
        {
        }

        /// <summary>
        /// Starts the authorization process.
        /// </summary>
        public override AuthorizationState StartAuthorization()
        {
            AuthorizationState restoredAuthorizationState;
            if (this.AuthorizationStore.TryRestoreAuthorization(out restoredAuthorizationState) && restoredAuthorizationState != null && !restoredAuthorizationState.IsExpired)
            {
                return restoredAuthorizationState;
            }

            var authorizationState = this.AuthorizationHandler.StartAuthorization();
            this.AuthorizationStore.StoreAuthorization(authorizationState);
            return authorizationState;
        }

        /// <summary>
        /// Revokes the authorization and clears also the <see cref="IAuthorizationStore"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">The access token can not be revoked due the lack of authorization. </exception>
        public override void RevokeAuthorization()
        {
            throw new NotSupportedException(ExceptionMessages.CantRevokeAuthorizationDueTheLackOfAuthorizationExceptionMessage);
        }

        /// <summary>
        /// An exception is thrown in this state.
        /// </summary>
        /// <returns>An exception.</returns>
        /// <exception cref="NotSupportedException">The Bearer-Authorization header can not be created due the lack of authorization.</exception>
        public override HttpHeader CreateBearer()
        {
            throw new NotSupportedException(ExceptionMessages.CantCreateBearerDueTheLackOfAuthorizationExceptionMessage);
        }

        /// <summary>
        /// Refreshes the authorization saved by the state.
        /// </summary>
        /// <exception cref="NotSupportedException">Authorization can not be refreshed due the lack of authorization.</exception>
        public override void RefreshAuthorization()
        {
            throw new NotSupportedException(ExceptionMessages.CantRefreshAuthorizationDueTheLackOfAuthorizationExceptionMessage);
        }
    }
}