namespace JanHafner.Restify.Services.OAuth2.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Configuration;

    /// <summary>
    /// Provides configuration for an authorization context.
    /// </summary>
    internal sealed class AuthorizationContextConfigurationConfigurationElement : ConfigurationElement, IAuthorizationContextConfiguration
    {
        /// <summary>
        /// Provides Google authorization information.
        /// </summary>
        public IAuthorizationConfiguration Authorization
        {
            get { return this.AuthorizationConfiguration; }
        }

        /// <summary>
        /// Internal method used for abstraction.
        /// </summary>
        [ConfigurationProperty("OAuth2", IsRequired = true)]
        public OAuth2ConfigurationElement AuthorizationConfiguration
        {
            get { return (OAuth2ConfigurationElement) this["OAuth2"]; }
        }

        /// <summary>
        /// The base <see cref="Uri"/> of the Google Api.
        /// </summary>
        [TypeConverter(typeof (UriTypeConverter))]
        [ConfigurationProperty("baseUrl", IsRequired = true)]
        public Uri BaseUrl
        {
            get { return (Uri) this["baseUrl"]; }
        }

        /// <summary>
        /// The Api key.
        /// </summary>
        [ConfigurationProperty("apiKey", IsRequired = true)]
        public String ApiKey
        {
            get { return (String) this["apiKey"]; }
        }

        /// <summary>
        /// The name of the context configuration.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get { return (String) this["name"]; }
        }

        /// <summary>
        /// Provides access to configuration for the <see cref="FileSystemAuthorizationStore"/>.
        /// </summary>
        public IFileSystemAuthorizationStoreConfiguration FileSystemAuthorizationStoreConfiguration
        {
            get { return this.FileSystemAccessTokenStoreConfigurationConfiguration; }
        }

        /// <summary>
        /// Internal method used for abstraction.
        /// </summary>
        [ConfigurationProperty("FileSystemAccessTokenStore")]
        internal FileSystemAuthorizationStoreConfigurationElement FileSystemAccessTokenStoreConfigurationConfiguration
        {
            get { return (FileSystemAuthorizationStoreConfigurationElement) this["FileSystemAccessTokenStore"]; }
        }
    }
}