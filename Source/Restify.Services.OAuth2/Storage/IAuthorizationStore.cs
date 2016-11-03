namespace JanHafner.Restify.Services.OAuth2.Storage
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Stores and restores the authorization.
    /// </summary>
    public interface IAuthorizationStore
    {
        /// <summary>
        /// Tries to restore the <see cref="AuthorizationState"/>.
        /// </summary>
        /// <param name="authorizationState">The <see cref="AuthorizationState"/>.</param>
        /// <returns>A value indicating if the restore was successful.</returns>
        Boolean TryRestoreAuthorization([CanBeNull] out AuthorizationState authorizationState);

        /// <summary>
        /// Stores the <see cref="AuthorizationState"/>.
        /// </summary>
        /// <param name="authorizationState">The <see cref="AuthorizationState"/> to store.</param>
        void StoreAuthorization([NotNull] AuthorizationState authorizationState);

        /// <summary>
        /// If necessary, clear all stored information about the authorization.
        /// </summary>
        void ClearStorage();
    }
}