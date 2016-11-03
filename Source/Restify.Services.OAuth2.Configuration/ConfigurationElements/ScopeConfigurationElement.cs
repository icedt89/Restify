namespace JanHafner.Restify.Services.OAuth2.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Configuration;

    /// <summary>
    /// The configuration element for a single Scope.
    /// </summary>
    internal sealed class ScopeConfigurationElement : ConfigurationElement, IScope
    {
        /// <summary>
        /// The scope which should be granted.
        /// </summary>
        [ConfigurationProperty("scope", IsRequired = true)]
        public String Scope
        {
            get { return (String) this["scope"]; }
        }

        /// <summary>
        /// Indicates if the scope should be granted.
        /// </summary>
        [TypeConverter(typeof (BooleanConverter))]
        [ConfigurationProperty("grant", DefaultValue = true)]
        public Boolean Grant
        {
            get { return (Boolean) this["grant"]; }
        }
    }
}