namespace JanHafner.Restify.Services.OAuth2.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Linq;

    /// <summary>
    /// Defines the OAuth2 Configuration Element.
    /// </summary>
    internal sealed class OAuth2ConfigurationElement : ConfigurationElement, IAuthorizationConfiguration
    {
        /// <summary>
        /// The <see cref="Uri"/> which points to the authorization endpoint of Google.
        /// </summary>
        [TypeConverter(typeof (UriTypeConverter))]
        [ConfigurationProperty("authorizationEndPoint", IsRequired = true)]
        public Uri AuthenticationEndPoint
        {
            get { return (Uri) this["authorizationEndPoint"]; }
        }

        /// <summary>
        /// The <see cref="Uri"/> which points to the toke endpoint of Google.
        /// </summary>
        [TypeConverter(typeof (UriTypeConverter))]
        [ConfigurationProperty("tokenEndPoint", IsRequired = true)]
        public Uri TokenEndPoint
        {
            get { return (Uri) this["tokenEndPoint"]; }
        }

        /// <summary>
        /// The <see cref="Uri"/> which points to the revocation endpoint of Google.
        /// </summary>
        [TypeConverter(typeof (UriTypeConverter))]
        [ConfigurationProperty("revocationEndPoint", IsRequired = true)]
        public Uri RevocationEndPoint
        {
            get { return (Uri) this["revocationEndPoint"]; }
        }

        public IX509CertificateConfiguration X509Certificate
        {
            get { return this.X509CertificateConfiguration; }
        }

        /// <summary>
        /// The X509 certificate for the authorization from the configuration file.
        /// </summary>
        [ConfigurationProperty("X509Certificate", IsRequired = true)]
        public X509CertificateConfigurationElement X509CertificateConfiguration
        {
            get { return (X509CertificateConfigurationElement) this["X509Certificate"]; }
        }

        /// <summary>
        /// The client Id.
        /// </summary>
        [ConfigurationProperty("clientId", IsRequired = true)]
        public String ClientId
        {
            get { return (String) this["clientId"]; }
        }

        /// <summary>
        /// The client secret.
        /// </summary>
        [ConfigurationProperty("clientSecret", IsRequired = true)]
        public String ClientSecret
        {
            get { return (String) this["clientSecret"]; }
        }

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> which defines the scopes for which the authorization should be ganted.
        /// </summary>
        public IEnumerable<IScope> Scopes
        {
            get { return this.ScopesConfiguration.Cast<IScope>(); }
        }

        /// <summary>
        /// The scopes for the authorization from the configuration file.
        /// </summary>
        [ConfigurationProperty("Scopes", IsRequired = true)]
        public ScopesConfigurationElementCollection ScopesConfiguration
        {
            get { return (ScopesConfigurationElementCollection) this["Scopes"]; }
        }
    }
}