namespace JanHafner.Restify.Services.OAuth2.UserInteractive
{
    using System;
    using System.Net.Http;
    using Configuration;
    using Handler;
    using JetBrains.Annotations;
    using Properties;
    using Queryfy;

    /// <summary>
    /// Handles the authorization.
    /// </summary>
    public sealed class UserInteractiveAuthorizationHandler : AuthorizationHandler
    {
        [NotNull]
        private readonly IAccessCodeHandler accessCodeRetriever;

        /// <summary>Initializes a new instance of the <see cref="UserInteractiveAuthorizationHandler"/> class.</summary>
        /// <param name="accessCodeRetriever">An implementation of the <see cref="IAccessCodeHandler"/> interface.</param>
        /// <param name="configuration">An instance of the <see cref="IAuthorizationContextConfiguration"/>.</param>
        /// <param name="queryfyDotNet">The <see cref="IQueryfyDotNet"/>.</param>
        /// <param name="httpClient">The <see cref="HttpClient"/>.</param>
        public UserInteractiveAuthorizationHandler([NotNull] IAccessCodeHandler accessCodeRetriever, 
            IAuthorizationContextConfiguration configuration, 
            IQueryfyDotNet queryfyDotNet,
            HttpClient httpClient)
            : base(configuration, queryfyDotNet, httpClient)
        {
            if (accessCodeRetriever == null)
            {
                throw new ArgumentNullException(nameof(accessCodeRetriever));
            }

            this.accessCodeRetriever = accessCodeRetriever;
        }

        /// <summary>
        /// Starts the authorization process.
        /// </summary>
        /// <returns>
        /// The <see cref="AuthorizationState"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">Access code could not be retrieved.</exception>
        public override AuthorizationState StartAuthorization()
        {
            var oAuth2EndpointBuilder = new OAuth2EndpointBuilder(this.AuthorizationContextConfiguration, this.QueryfyDotNet);

            var requestUri = oAuth2EndpointBuilder.BuildAuthenticationEndpoint();
            var accessCode = this.accessCodeRetriever.GetAccessCode(requestUri);
            if (string.IsNullOrEmpty(accessCode))
            {
                throw new InvalidOperationException(ExceptionMessages.AccessCodeCouldNotBeRetrievedExceptionMessage);
            }

            using (var tokenRequest = oAuth2EndpointBuilder.BuildTokenRequest(accessCode))
            {
                return tokenRequest.GetAuthorizationState(this.HttpClient, DateTime.UtcNow, this.AuthorizationContextConfiguration.Authorization.Scopes.GetGrantedScopes());
            }
        }
    }
}