namespace JanHafner.Restify.Services.OAuth2.Configuration
{
    using System;
    using System.Configuration;
    using System.IO;

    /// <summary>
    /// Defines the FileSystemAuthorizationStore Configuration Element.
    /// </summary>
    internal sealed class FileSystemAuthorizationStoreConfigurationElement : ConfigurationElement, IFileSystemAuthorizationStoreConfiguration
    {
        /// <summary>
        /// A absolute or relative file path where the authorization is stored. Environment variables are welcome.
        /// </summary>
        [ConfigurationProperty("filePath", IsRequired = true)]
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
    }
}