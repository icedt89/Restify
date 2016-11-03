namespace JanHafner.Restify.Services.OAuth2.Storage
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Does not persist any information.
    /// </summary>
    public sealed class NullAuthorizationStore : IAuthorizationStore
    {
        /// <summary>
        /// Tries to restore the <see cref="AuthorizationState"/>.
        /// </summary>
        /// <param name="authorizationState">The <see cref="AuthorizationState"/>.</param>
        /// <returns>A value indicating if the restore was successful.</returns>
        public Boolean TryRestoreAuthorization(out AuthorizationState authorizationState)
        {
            authorizationState = null;
            return false;
        }

        /// <summary>
        /// Stores the <see cref="AuthorizationState"/>.
        /// </summary>
        /// <param name="authorizationState">The <see cref="AuthorizationState"/> to store.</param>
        public void StoreAuthorization([CanBeNull] AuthorizationState authorizationState)
        {
        }

        /// <summary>
        /// If necessary, clear all stored information about the authorization.
        /// </summary>
        public void ClearStorage()
        {
        }
    }
}