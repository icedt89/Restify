namespace JanHafner.Restify.Services.OAuth2.UserInteractive
{
    using System;
    using System.Net.Http;
    using System.Text;
    using Configuration;
    using JetBrains.Annotations;
    using Queryfy;

    /// <summary>
    /// Builds the endpoints for the <see cref="UserInteractiveAuthorizationHandler"/>.
    /// </summary>
    internal sealed class OAuth2EndpointBuilder
    {
        private readonly IAuthorizationContextConfiguration authorizationContextConfiguration;
        
        private readonly IQueryfyDotNet queryfyDotNet;

        private const String RedirectUri = "urn:ietf:wg:oauth:2.0:oob";

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2EndpointBuilder"/> class.
        /// </summary>
        /// <param name="authorizationContextConfiguration">The <see cref="IAuthorizationContextConfiguration"/>.</param>
        /// <param name="queryfyDotNet">An instance of <see cref="IQueryfyDotNet"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="authorizationContextConfiguration"/>' and '<paramref name="queryfyDotNet"/>' cannot be null. </exception>
        public OAuth2EndpointBuilder([NotNull] IAuthorizationContextConfiguration authorizationContextConfiguration, [NotNull] IQueryfyDotNet queryfyDotNet)
        {
            if (authorizationContextConfiguration == null)
            {
                throw new ArgumentNullException(nameof(authorizationContextConfiguration));
            }

            if (queryfyDotNet == null)
            {
                throw new ArgumentNullException(nameof(queryfyDotNet));
            }

            this.authorizationContextConfiguration = authorizationContextConfiguration;
            this.queryfyDotNet = queryfyDotNet;
        }

        /// <summary>
        /// Builds a <see cref="HttpRequestMessage"/> for requesting the token.
        /// </summary>
        /// <param name="accessCode">The access code.</param>
        /// <exception cref="ArgumentNullException">The value of 'accessCode' cannot be null. </exception>
        /// <returns>The <see cref="HttpRequestMessage"/> for the token request.</returns>
        [NotNull]
        public HttpRequestMessage BuildTokenRequest([NotNull] String accessCode)
        {
            if (String.IsNullOrEmpty(accessCode))
            {
                throw new ArgumentNullException(nameof(accessCode));
            }

            var result = new HttpRequestMessage(HttpMethod.Post, this.authorizationContextConfiguration.Authorization.TokenEndPoint);

            var content = this.queryfyDotNet.Queryfy(new
                                                     {
                                                         code = accessCode,
                                                         client_id = this.authorizationContextConfiguration.Authorization.ClientId,
                                                         client_secret = this.authorizationContextConfiguration.Authorization.ClientSecret,
                                                         redirect_uri = RedirectUri,
                                                         grant_type = "authorization_code"
                                                     }).QueryString;
            result.Content = new StringContent(content, Encoding.Default, "application/x-www-form-urlencoded");

            return result;
        }

        /// <summary>
        /// Builds the authentication endpoint.
        /// </summary>
        /// <returns>The <see cref="Uri"/> for the authentication endpoint.</returns>
        [NotNull]
        public Uri BuildAuthenticationEndpoint()
        {
            return new UriBuilder(this.authorizationContextConfiguration.Authorization.AuthenticationEndPoint.ToString())
                   {
                       Query = this.queryfyDotNet.Queryfy(new
                                                          {
                                                              redirect_uri = RedirectUri,
                                                              response_type = "code",
                                                              client_id = this.authorizationContextConfiguration.Authorization.ClientId,
                                                              scope = this.authorizationContextConfiguration.Authorization.Scopes.GetGrantedScopes()
                                                          }).QueryString
                   }.Uri;
        }
    }
}