namespace JanHafner.Restify.Services.OAuth2.Configuration
{
    using System;
    using System.Configuration;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="AuthorizationContextConfigurationsElementCollection"/> class.
    /// </summary>
    [UsedImplicitly]
    internal sealed class AuthorizationContextConfigurationsElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationContextConfigurationsElementCollection"/> class.
        /// </summary>
        public AuthorizationContextConfigurationsElementCollection()
        {
            this.AddElementName = "AuthorizationContext";
        }

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new AuthorizationContextConfigurationConfigurationElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for. </param>
        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((AuthorizationContextConfigurationConfigurationElement)element).Name;
        }
    }
}