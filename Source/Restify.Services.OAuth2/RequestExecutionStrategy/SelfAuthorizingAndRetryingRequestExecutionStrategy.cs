namespace JanHafner.Restify.Services.OAuth2.RequestExecutionStrategy
{
    using System;
    using System.Net;
    using Request;
    using Response;
    using AuthorizationContext;
    using Services.RequestExecutionStrategy;
    using JetBrains.Annotations;

    /// <summary>
    /// If the <see cref="IAuthorizationContext"/> is not authorized, than the authorization process will be started. Additionally exceptions that are identified as "NotAuthorized" will be
    /// catched  and the authorization process is restarted.
    /// </summary>
    public sealed class SelfAuthorizingAndRetryingRequestExecutionStrategy : IRequestExecutionStrategy
    {
        [NotNull]
        private readonly IAuthorizationContext authorizationContext;

        [NotNull]
        private readonly IRequestProcessor requestProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfAuthorizingRequestExecutionStrategy"/> class.
        /// </summary>
        /// <param name="requestProcessor">The <see cref="IRequestProcessor"/>.</param>
        /// <param name="authorizationContext">The <see cref="IAuthorizationContext"/> that is used to handle the authorization.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="authorizationContext"/>' and '<paramref name="requestProcessor"/>' cannot be null. </exception>
        public SelfAuthorizingAndRetryingRequestExecutionStrategy([NotNull] IRequestProcessor requestProcessor, [NotNull] IAuthorizationContext authorizationContext)
        {
            if (requestProcessor == null)
            {
                throw new ArgumentNullException(nameof(requestProcessor));
            }

            if (authorizationContext == null)
            {
                throw new ArgumentNullException(nameof(authorizationContext));
            }

            this.requestProcessor = requestProcessor;
            this.authorizationContext = authorizationContext;
        }

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <typeparam name="TResponse">The result type of the response.</typeparam>
        /// <typeparam name="TRequest">The request.</typeparam>
        /// <param name="request">The request to process.</param>
        /// <returns>The deserialized response.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="request"/>' cannot be null. </exception>
        public TResponse Execute<TResponse, TRequest>(TRequest request)
            where TResponse : ResponseBase<TRequest>
            where TRequest : RequestBase
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!this.authorizationContext.IsAuthorized)
            {
                this.authorizationContext.StartAuthorization();
            }

            this.authorizationContext.EnsureAuthorization(request);

            try
            {
                return this.requestProcessor.ProcessRequest<TResponse, TRequest>(request);
            }
            catch (HttpRequestExecutionException httpRequestExecutionException)
            {
                if (httpRequestExecutionException.HttpStatusCode == (Int32)HttpStatusCode.Unauthorized)
                {
                    this.authorizationContext.RevokeAuthorization();
                    this.authorizationContext.StartAuthorization();

                    this.authorizationContext.EnsureAuthorization(request);

                    return this.requestProcessor.ProcessRequest<TResponse, TRequest>(request);
                }

                throw;
            }
        }
    }
}