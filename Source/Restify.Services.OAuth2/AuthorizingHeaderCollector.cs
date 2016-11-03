namespace JanHafner.Restify.Services.OAuth2
{
    using System;
    using Header;
    using Request;
    using AuthorizationContext;
    using JetBrains.Annotations;

    /// <summary>
    /// Adds the Bearer-authorization header to the request if necessary.
    /// </summary>
    public sealed class AuthorizingHeaderCollector : IRequestHeaderCollector
    {
        [NotNull]
        private readonly IAuthorizationContext authorizationContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizingHeaderCollector"/> class.
        /// </summary>
        /// <param name="authorizationContext">The <see cref="IAuthorizationContext"/>.</param>
        /// <exception cref="ArgumentNullException">The value of 'auth<paramref name="authorizationContext"/>orizationContext' cannot be null. </exception>
        public AuthorizingHeaderCollector([NotNull] IAuthorizationContext authorizationContext)
        {
            if (authorizationContext == null)
            {
                throw new ArgumentNullException(nameof(authorizationContext));
            }

            this.authorizationContext = authorizationContext;
        }

        /// <summary>
        /// Inspects the <see cref="RequestBase"/> and creates a <see cref="HttpHeaderCollection"/> with applicable headers.
        /// </summary>
        /// <param name="request">The <see cref="RequestBase"/>.</param>
        /// <param name="collectedHttpHeaders"></param>
        /// <returns>A new instance of the <see cref="HttpHeaderCollection"/> class.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="request"/>' and '<paramref name="collectedHttpHeaders"/>' cannot be null. </exception>
        public void CollectRequestHeaders(RequestBase request, HttpHeaderCollection collectedHttpHeaders)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (collectedHttpHeaders == null)
            {
                throw new ArgumentNullException(nameof(collectedHttpHeaders));
            }

            // ReSharper disable once SuspiciousTypeConversion.Global Third-party developers can provide the implementation.
            if (request is INeedAuthorization)
            {
                collectedHttpHeaders.Add(this.authorizationContext.CreateBearer());
            }
        }
    }
}