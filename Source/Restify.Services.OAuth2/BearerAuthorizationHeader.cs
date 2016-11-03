namespace JanHafner.Restify.Services.OAuth2
{
    using System;
    using Header;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="HttpHeader"/> for OAuth2 authorization.
    /// </summary>
    public sealed class BearerAuthorizationHeader : HttpHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BearerAuthorizationHeader"/> class.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        public BearerAuthorizationHeader([CanBeNull] String accessToken)
            : base("Authorization", String.Format("Bearer {0}", accessToken))
        {
        }
    }
}