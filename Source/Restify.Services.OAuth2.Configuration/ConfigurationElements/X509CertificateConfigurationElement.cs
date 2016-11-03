namespace JanHafner.Restify.Services.OAuth2.Configuration
{
    using System;
    using System.Configuration;
    using System.IO;
    using JetBrains.Annotations;

    /// <summary>
    /// The configuration element for the X509 certificate.
    /// </summary>
    internal sealed class X509CertificateConfigurationElement : ConfigurationElement, IX509CertificateConfiguration
    {
        /// <summary>
        /// The file path for the X509 cretificate file.
        /// </summary>
        [ConfigurationProperty("filePath", DefaultValue = null)]
        public String FilePath
        {
            get
            {
                var rawValue = (String) this["filePath"];
                if (!String.IsNullOrWhiteSpace(rawValue))
                {
                    return Path.GetFullPath(rawValue);
                }

                return null;
            }
        }

        /// <summary>
        /// If the file path is not specified this value should be filled.
        /// </summary>
        public Byte[] CertificateContent
        {
            get
            {
                var rawValue = this.RawContent;
                if (String.IsNullOrWhiteSpace(rawValue))
                {
                    return null;
                }

                return Convert.FromBase64String(rawValue);
            }
        }

        /// <summary>
        /// If the file path is not specified this value should be filled.
        /// </summary>
        [ConfigurationProperty("content", DefaultValue = null)]
        [CanBeNull]
        private String RawContent
        {
            get { return (String) this["content"]; }
        }
    }
}