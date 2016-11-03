namespace JanHafner.Restify.Services.OAuth2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    /// <summary>
    /// Saves the state of the access token.
    /// </summary>
    [Serializable]
    public sealed class AuthorizationState : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationState"/> class.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="expires">The timespan the access token expires.</param>
        /// <param name="tokenType">The type of the access token.</param>
        /// <param name="grantedOnUtc">The time on which the access token was granted.</param>
        /// <param name="scopes">The scopes for which the access token is obtained.</param>
        public AuthorizationState([CanBeNull] String accessToken, [CanBeNull] String refreshToken, TimeSpan expires, [CanBeNull] String tokenType, DateTime grantedOnUtc, [CanBeNull] IEnumerable<String> scopes)
        {
            this.Scopes = scopes ?? Enumerable.Empty<String>();
            this.TokenType = tokenType;
            this.RefreshToken = refreshToken;
            this.GrantedOnUtc = grantedOnUtc;
            this.AccessToken = accessToken;
            this.Expires = expires;
            this.ExpiresOnUtc = this.GrantedOnUtc.Add(expires);
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        [CanBeNull]
        public String AccessToken { get; private set; }

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        [CanBeNull]
        public String RefreshToken { get; private set; }

        /// <summary>
        /// Gets the type of the token.
        /// </summary>
        [CanBeNull]
        public String TokenType { get; private set; }

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> in which timespan the access token expires.
        /// </summary>
        public TimeSpan Expires { get; private set; }

        /// <summary>
        /// Gets the <see cref="DateTime"/> on which the access token was granted.
        /// </summary>
        public DateTime GrantedOnUtc { get; private set; }

        /// <summary>
        /// Gets the <see cref="DateTime"/> on which the access token will expires.
        /// </summary>
        public DateTime ExpiresOnUtc { get; private set; }

        /// <summary>
        /// Gets the scopes for which the access token is obtained.
        /// </summary>
        [NotNull]
        public IEnumerable<String> Scopes { get; private set; }

        /// <summary>
        /// Gets a value which indicates if the access token is expired.
        /// </summary>
        public Boolean IsExpired
        {
            get
            {
                return this.ExpiresOnUtc <= DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Gets a value which indicates if this authorization is refreshable.
        /// </summary>
        public Boolean IsRefreshable
        {
            get
            {
                return String.IsNullOrEmpty(this.RefreshToken);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.AccessToken = null;
            this.TokenType = null;
            this.RefreshToken = null;
            this.Scopes = Enumerable.Empty<String>();
            this.ExpiresOnUtc = DateTime.MinValue;
        }
    }
}