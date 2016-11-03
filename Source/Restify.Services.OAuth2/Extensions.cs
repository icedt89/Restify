namespace JanHafner.Restify.Services.OAuth2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Configuration;
    using JetBrains.Annotations;
    using Newtonsoft.Json;

    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets a list of all granted scopes as <see cref="IEnumerable{String}"/>.
        /// </summary>
        /// <param name="scopes">The scopes.</param>
        /// <returns>All granted scopes as <see cref="IEnumerable{String}"/></returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="scopes"/>' cannot be null. </exception>
        [NotNull]
        public static IEnumerable<String> GetGrantedScopes([NotNull] this IEnumerable<IScope> scopes)
        {
            if (scopes == null)
            {
                throw new ArgumentNullException(nameof(scopes));
            }

            return scopes.Where(scope => scope.Grant).Select(scope => scope.Scope);
        }

        /// <summary>
        /// Creates an <see cref="AuthorizationState"/> from the supplied json.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="issueTime">The time on which the token was issued.</param>
        /// <param name="grantedScopes">All scopes for which the token is granted.</param>
        /// <returns>The <see cref="AuthorizationState"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="response"/>' cannot be null. </exception>
        [NotNull]
        private static AuthorizationState GetAuthorizationStateFromTokenResponse([NotNull] this String response, DateTime issueTime, [CanBeNull] IEnumerable<String> grantedScopes)
        {
            if (String.IsNullOrEmpty(response))
            {
                throw new ArgumentNullException(nameof(response));
            }

            var jsonResponse = JsonConvert.DeserializeAnonymousType(
                          response,
                          new
                          {
                              access_token = String.Empty,
                              token_type = "Bearer",
                              expires_in = 0,
                              refresh_token = String.Empty
                          });

            return new AuthorizationState(jsonResponse.access_token, jsonResponse.refresh_token, TimeSpan.FromSeconds(jsonResponse.expires_in), jsonResponse.token_type, issueTime, grantedScopes);
        }

        /// <summary>
        /// Helper method for requesting an <see cref="AuthorizationState"/> 
        /// </summary>
        /// <returns>The <see cref="AuthorizationState"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="tokenRequest"/>' and '<paramref name="httpClient"/>' cannot be null. </exception>
        [NotNull]
        public static AuthorizationState GetAuthorizationState([NotNull] this HttpRequestMessage tokenRequest, [NotNull] HttpClient httpClient, DateTime issueTime, [CanBeNull] IEnumerable<String> grantedScopes)
        {
            if (tokenRequest == null)
            {
                throw new ArgumentNullException(nameof(tokenRequest));
            }

            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            using (var tokenResponse = httpClient.SendAsync(tokenRequest).Result)
            {
                var responseBody = tokenResponse.Content.ReadAsStringAsync().Result;
                return responseBody.GetAuthorizationStateFromTokenResponse(issueTime, grantedScopes);
            }
        }
    }
}