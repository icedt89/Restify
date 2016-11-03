namespace JanHafner.Restify.Services.OAuth2.Configuration
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Provides configuration for the <see cref="IFileSystemAuthorizationStoreConfiguration"/>
    /// </summary>
    public interface IFileSystemAuthorizationStoreConfiguration
    {
        /// <summary>
        /// A absolute or relative file path where the authorization is stored. Environment variables are welcome.
        /// </summary>
        [CanBeNull]
        String FilePath { get; }
    }
}