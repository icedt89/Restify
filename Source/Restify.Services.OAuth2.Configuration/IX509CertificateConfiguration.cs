namespace JanHafner.Restify.Services.OAuth2.Configuration
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Contains information about the X509 certificate.
    /// </summary>
    public interface IX509CertificateConfiguration
    {
        /// <summary>
        /// The file path for the X509 cretificate file.
        /// </summary>
        [CanBeNull]
        String FilePath { get; }

        /// <summary>
        /// If the file path is not specified this value should be filled.
        /// </summary>
        [CanBeNull]
        Byte[] CertificateContent { get; }
    }
}