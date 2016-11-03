namespace JanHafner.Restify.Services.OAuth2.UserInteractive
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Provides methods for acquiring access codes.
    /// </summary>
    public interface IAccessCodeHandler
    {
        /// <summary>
        /// Gets the access code.
        /// </summary>
        /// <param name="authorizationUri">
        /// The Uri from which the access code is to retrieve.
        /// </param>
        /// <returns>
        /// The access code.
        /// </returns>
        [CanBeNull]
        string GetAccessCode([NotNull] Uri authorizationUri);
    }
}