namespace JanHafner.Restify.Services.OAuth2.AuthorizationContext
{
    using System;
    using System.Net;
    using System.Net.Http;
    using Configuration;
    using Handler;
    using Header;
    using JetBrains.Annotations;
    using Newtonsoft.Json;
    using Properties;
    using Storage;

    /// <summary>
    /// Defines the state where the <see cref="IAuthorizationContext"/> is authorized.
    /// </summary>
    public sealed class AuthorizedAuthorizationContextState : AuthorizationContextState
    {
        [NotNull]
        private AuthorizationState inMemoryAuthorizationState;

        [NotNull]
        private readonly IAuthorizationConfiguration authorizationConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizedAuthorizationContextState"/> class.
        /// </summary>
        /// <param name="inMemoryAuthorizationState">The in-memory authorization state.</param>
        /// <param name="authorizationStore">The <see cref="IAuthorizationStore"/>.</param>
        /// <param name="authorizationHandler">The <see cref="IAuthorizationHandler"/>.</param>
        /// <param name="authorizationConfiguration">The <see cref="IAuthorizationConfiguration"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="authorizationHandler" />', '<paramref name="inMemoryAuthorizationState"/>', '<paramref name="authorizationConfiguration"/>' and '<paramref name="authorizationStore" />' cannot be null. </exception>
        public AuthorizedAuthorizationContextState(AuthorizationState inMemoryAuthorizationState, IAuthorizationStore authorizationStore, IAuthorizationHandler authorizationHandler, IAuthorizationConfiguration authorizationConfiguration)
            : base(authorizationStore, authorizationHandler)
        {
            if (inMemoryAuthorizationState == null)
            {
                throw new ArgumentNullException(nameof(inMemoryAuthorizationState));
            }
            
            if (authorizationConfiguration == null)
            {
                throw new ArgumentNullException(nameof(authorizationConfiguration));
            }

            this.inMemoryAuthorizationState = inMemoryAuthorizationState;
            this.authorizationConfiguration = authorizationConfiguration;
        }

        /// <summary>
        /// Starts the authorization process.
        /// </summary>
        /// <exception cref="NotSupportedException">The context is already authorized. </exception>
        public override AuthorizationState StartAuthorization()
        {
            throw new NotSupportedException(ExceptionMessages.AlreadyAuthorizedExceptionMessage);
        }

        /// <summary>
        /// Revokes the authorization and clears also the <see cref="IAuthorizationStore"/>.
        /// </summary>
        public override void RevokeAuthorization()
        {
            var revokeUrl = String.Format(this.authorizationConfiguration.RevocationEndPoint.OriginalString, this.inMemoryAuthorizationState.AccessToken);
            using (var httpClient = new HttpClient())
            {
                var result = httpClient.GetAsync(revokeUrl, HttpCompletionOption.ResponseContentRead).Result;
                if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    var content = result.Content.ReadAsStringAsync().Result;
                    var errorContent = new
                    {
                        error = String.Empty
                    };
                    errorContent = JsonConvert.DeserializeAnonymousType(content, errorContent);
                    if (errorContent.error != "invalid_token")
                    {
                        result.EnsureSuccessStatusCode();
                    }
                }
            }

            this.inMemoryAuthorizationState.Dispose();
        }

        /// <summary>
        /// Creates a Bearer-authorization header.
        /// </summary>
        /// <returns>The Bearer-authorization header.</returns>
        public override HttpHeader CreateBearer()
        {
            return new BearerAuthorizationHeader(this.inMemoryAuthorizationState.AccessToken);
        }

        /// <summary>
        /// Refreshes the authorization saved by the state.
        /// </summary>
        public override void RefreshAuthorization()
        {
            this.inMemoryAuthorizationState = this.AuthorizationHandler.RefreshAuthorization(this.inMemoryAuthorizationState); 
            this.AuthorizationStore.StoreAuthorization(this.inMemoryAuthorizationState);
        }
    }
}