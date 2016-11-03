namespace JanHafner.Restify.Services
{
    using System;
    using JetBrains.Annotations;
    using RequestExecutionStrategy;
    using Request;
    using Response;

    /// <summary>
    /// Provides simple Rest-Service without authentication.
    /// </summary>
    public abstract class RestService : IRestService
    {
        [NotNull]
        private readonly IRequestExecutionStrategy requestExecutionStrategy;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestService"/> class.
        /// </summary>
        /// <param name="requestExecutionStrategy">The <see cref="IRequestExecutionStrategy"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="requestExecutionStrategy"/>' cannot be null. </exception>
        protected RestService([NotNull] IRequestExecutionStrategy requestExecutionStrategy)
        {
            if (requestExecutionStrategy == null)
            {
                throw new ArgumentNullException(nameof(requestExecutionStrategy));
            }

            this.requestExecutionStrategy = requestExecutionStrategy;
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <typeparam name="TResponse">The expected response.</typeparam>
        /// <typeparam name="TRequest">The request.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="request"/>' cannot be null. </exception>
        public TResponse ProcessRequest<TResponse, TRequest>(TRequest request)
            where TResponse : ResponseBase<TRequest>
            where TRequest : RequestBase
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var preProcessedRequest = this.PreProcessRequest(request);
            var processedResponse = this.requestExecutionStrategy.Execute<TResponse, TRequest>(preProcessedRequest);
            var postProcessedResponse = this.PostProcessResponse<TResponse, TRequest>(processedResponse);

            return postProcessedResponse;
        }

        /// <summary>
        /// Is called bevor the request is send, so extra behavior can be applied on service level.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>The request that is send.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="request"/>' cannot be null. </exception>
        [NotNull]
        protected virtual TRequest PreProcessRequest<TRequest>([NotNull] TRequest request)
            where TRequest : RequestBase
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return request;
        }

        /// <summary>
        /// Is called after the request is send and before the response returns from the service.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="response">The response.</param>
        /// <returns>The response that should be returned.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="response"/>' cannot be null. </exception>
        [NotNull]
        protected virtual TResponse PostProcessResponse<TResponse, TRequest>([NotNull] TResponse response)
            where TResponse : ResponseBase<TRequest>
            where TRequest : RequestBase
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return response;
        }
    }
}