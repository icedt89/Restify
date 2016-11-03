namespace JanHafner.Restify.Services.OAuth2.Configuration
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines the authorization contextn configuration.
    /// </summary>
    public interface IAuthorizationContextConfiguration
    {
        /// <summary>
        /// Provides Google authorization information.
        /// </summary>
        [CanBeNull]
        IAuthorizationConfiguration Authorization { get; }

        /// <summary>
        /// The base <see cref="Uri"/> of the Google Api.
        /// </summary>
        [NotNull]
        Uri BaseUrl { get; }

        /// <summary>
        /// The Api key.
        /// </summary>
        [CanBeNull]
        String ApiKey { get; }

        /// <summary>
        /// The name of the context configuration.
        /// </summary>
        [NotNull]
        String Name { get; }

        /// <summary>
        /// Provides access to configuration for the <see cref="FileSystemAuthorizationStore"/>.
        /// </summary>
        [CanBeNull]
        IFileSystemAuthorizationStoreConfiguration FileSystemAuthorizationStoreConfiguration { get; }
    }
}