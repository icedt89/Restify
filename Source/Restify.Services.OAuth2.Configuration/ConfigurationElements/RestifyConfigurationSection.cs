namespace JanHafner.Restify.Services.OAuth2.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="RestifyConfigurationSection"/> class type.
    /// </summary>
    public sealed class RestifyConfigurationSection : ConfigurationSection, IRestifyConfiguration
    {
        /// <summary>
        /// The scopes for the authorization from the configuration file.
        /// </summary>
        [ConfigurationProperty("AuthorizationContextConfigurations", IsRequired = true)]
        [NotNull]
        internal AuthorizationContextConfigurationsElementCollection AuthorizationContextConfigurationsElementCollection
        {
            get { return (AuthorizationContextConfigurationsElementCollection) this["AuthorizationContextConfigurations"]; }
        }

        /// <summary>
        /// Gets a list of all registered <see cref="IAuthorizationContextConfiguration"/>s.
        /// </summary>
        public IEnumerable<IAuthorizationContextConfiguration> AuthorizationContextConfigurations
        {
            get { return this.AuthorizationContextConfigurationsElementCollection.Cast<IAuthorizationContextConfiguration>(); }
        }

        /// <summary>
        /// Gets the configuration section.
        /// </summary>
        /// <returns></returns>
        internal static RestifyConfigurationSection GetConfigurationSection()
        {
            return (RestifyConfigurationSection)ConfigurationManager.GetSection("Restify");
        }

        /// <summary>
        /// Returns the section as an <see cref="IRestifyConfiguration"/>.
        /// </summary>
        /// <returns>The <see cref="IRestifyConfiguration"/>.</returns>
        public static IRestifyConfiguration GetXmlConfiguration()
        {
            return GetConfigurationSection();
        }

        /// <summary>
        /// Gets the <see cref="IAuthorizationContextConfiguration"/> with the supplied name.
        /// </summary>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="name"/>' cannot be null. </exception>
        public IAuthorizationContextConfiguration GetAuthorizationContextConfiguration(String name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return this.AuthorizationContextConfigurations.Single(a => a.Name == name);
        }
    }
}