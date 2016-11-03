namespace JanHafner.Restify.Services.OAuth2.RequestExecutionStrategy
{
    using System;
    using AuthorizationContext;
    using JetBrains.Annotations;
    using Request;
    using Response;
    using Services.RequestExecutionStrategy;

    /// <summary>
    /// If the <see cref="IAuthorizationContext"/> is not authorized, than the authorization process will be started.
    /// </summary>
    public sealed class SelfAuthorizingRequestExecutionStrategy : IRequestExecutionStrategy
    {
        [NotNull]
        private readonly IRequestProcessor requestProcessor;

        [NotNull]
        private readonly IAuthorizationContext authorizationContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfAuthorizingRequestExecutionStrategy"/> class.
        /// </summary>
        /// <param name="requestProcessor">The <see cref="IRequestProcessor"/>.</param>
        /// <param name="authorizationContext">The <see cref="IAuthorizationContext"/> that is used to handle the authorization.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="requestProcessor"/>' and '<paramref name="authorizationContext"/>' cannot be null. </exception>
        public SelfAuthorizingRequestExecutionStrategy(IRequestProcessor requestProcessor, IAuthorizationContext authorizationContext)
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

            return this.requestProcessor.ProcessRequest<TResponse, TRequest>(request);
        }
    }
}