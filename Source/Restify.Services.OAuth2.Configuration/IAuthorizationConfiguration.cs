namespace JanHafner.Restify.Services.OAuth2.Configuration
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    /// Provides information about the authorization process.
    /// </summary>
    public interface IAuthorizationConfiguration
    {
        /// <summary>
        /// The <see cref="Uri"/> which points to the authorization endpoint of Google.
        /// </summary>
        [NotNull]
        Uri AuthenticationEndPoint { get; }

        /// <summary>
        /// The <see cref="Uri"/> which points to the token endpoint of Google.
        /// </summary>
        [NotNull]
        Uri TokenEndPoint { get; }

        /// <summary>
        /// The <see cref="Uri"/> which points to the revocation endpoint of Google.
        /// </summary>
        [NotNull]
        Uri RevocationEndPoint { get; }

        /// <summary>
        /// The X509 certificate file for Service Account authorization.
        /// </summary>
        [CanBeNull]
        IX509CertificateConfiguration X509Certificate { get; }

        /// <summary>
        /// The client Id.
        /// </summary>
        [NotNull]
        String ClientId { get; }

        /// <summary>
        /// The client secret.
        /// </summary>
        [NotNull]
        String ClientSecret { get; }

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> which defines the scopes for which the authorization should be ganted.
        /// </summary>
        [NotNull]
        IEnumerable<IScope> Scopes { get; }
    }
}