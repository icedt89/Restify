namespace JanHafner.Restify.Services.RequestExecutionStrategy
{
    using System;
    using JetBrains.Annotations;
    using Request;
    using Response;

    /// <summary>
    /// Default request execution strategy, simply executes the request using the <see cref="IRequestProcessor"/>.
    /// </summary>
    public sealed class DefaultRequestExecutionStrategy : IRequestExecutionStrategy
    {
        [NotNull]
        private readonly IRequestProcessor requestProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRequestExecutionStrategy"/> class.
        /// </summary>
        /// <param name="requestProcessor">The <see cref="IRequestProcessor"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="requestProcessor"/>' cannot be null. </exception>
        public DefaultRequestExecutionStrategy([NotNull] IRequestProcessor requestProcessor)
        {
            if (requestProcessor == null)
            {
                throw new ArgumentNullException(nameof(requestProcessor));
            }

            this.requestProcessor = requestProcessor;
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

            return this.requestProcessor.ProcessRequest<TResponse, TRequest>(request);
        }
    }
}