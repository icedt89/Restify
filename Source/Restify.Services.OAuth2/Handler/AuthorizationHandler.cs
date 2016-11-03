namespace JanHafner.Restify.Services.OAuth2.Handler
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using Configuration;
    using JetBrains.Annotations;
    using Queryfy;

    /// <summary>
    /// This base class provides the ability to refresh the supplied access token.
    /// </summary>
    public abstract class AuthorizationHandler : IAuthorizationHandler
    {
        /// <summary>
        /// The <see cref="IAuthorizationContextConfiguration"/>.
        /// </summary>
        [NotNull]
        protected readonly IAuthorizationContextConfiguration AuthorizationContextConfiguration;

        /// <summary>
        /// The <see cref="IQueryfyDotNet"/>.
        /// </summary>
        [NotNull]
        protected readonly IQueryfyDotNet QueryfyDotNet;

        /// <summary>
        /// The <see cref="HttpClient"/>.
        /// </summary>
        [NotNull]
        protected readonly HttpClient HttpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationHandler"/> class.
        /// </summary>
        /// <param name="authorizationContextConfiguration">The <see cref="IAuthorizationContextConfiguration"/>.</param>
        /// <param name="queryfyDotNet">The <see cref="IQueryfyDotNet"/>.</param>
        /// <param name="httpClient">The <see cref="HttpClient"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="httpClient"/>', '<paramref name="authorizationContextConfiguration"/>' and '<paramref name="queryfyDotNet"/>' cannot be null. </exception>
        protected AuthorizationHandler([NotNull] IAuthorizationContextConfiguration authorizationContextConfiguration, [NotNull] IQueryfyDotNet queryfyDotNet, [NotNull] HttpClient httpClient)
        {
            if (authorizationContextConfiguration == null)
            {
                throw new ArgumentNullException(nameof(authorizationContextConfiguration));
            }

            if (queryfyDotNet == null)
            {
                throw new ArgumentNullException(nameof(queryfyDotNet));
            }

            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            this.AuthorizationContextConfiguration = authorizationContextConfiguration;
            this.QueryfyDotNet = queryfyDotNet;
            this.HttpClient = httpClient;
        }

        /// <summary>
        /// Begins the authorization process.
        /// </summary>
        [NotNull]
        public abstract AuthorizationState StartAuthorization();

        /// <summary>
        /// Refreshs the supplied authorization using the refresh token from the <see cref="AuthorizationState"/>.
        /// </summary>
        /// <param name="current">The authorization that should be refreshed.</param>
        /// <returns>The refreshed authorization.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="current"/>' cannot be null. </exception>
        [NotNull]
        public AuthorizationState RefreshAuthorization(AuthorizationState current)
        {
            if (current == null)
            {
                throw new ArgumentNullException(nameof(current));
            }

            if (current.IsRefreshable)
            {
                var query = this.QueryfyDotNet.Queryfy(new
                                                     {
                                                         grant_type = "refresh_token",
                                                         refresh_token = current.RefreshToken,
                                                         client_id = this.AuthorizationContextConfiguration.Authorization.ClientId,
                                                         client_secret = this.AuthorizationContextConfiguration.Authorization.ClientSecret
                                                     });
                using (var tokenRequest = new HttpRequestMessage(HttpMethod.Post, this.AuthorizationContextConfiguration.Authorization.TokenEndPoint)
                {
                    Content = new StringContent(query.EncodedQueryString, Encoding.UTF8, "application/x-www-form-urlencoded")
                })
                {
                    return tokenRequest.GetAuthorizationState(this.HttpClient, DateTime.UtcNow, this.AuthorizationContextConfiguration.Authorization.Scopes.GetGrantedScopes().ToList());
                }
            }

            return this.StartAuthorization();
        }
    }
}