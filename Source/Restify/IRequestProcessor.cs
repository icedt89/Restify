namespace JanHafner.Restify
{
    using System;
    using JetBrains.Annotations;
    using Request;
    using Response;

    /// <summary>
    /// Provides methods for processing REST requests.
    /// </summary>
    public interface IRequestProcessor : IDisposable
    {
        /// <summary>
        /// Processes the request and returns the response.
        /// </summary>
        /// <typeparam name="TResponse">The result type of the response.</typeparam>
        /// <typeparam name="TRequest">The request.</typeparam>
        /// <param name="request">The request to process.</param>
        /// <returns>The deserialized response.</returns>
        [NotNull]
        TResponse ProcessRequest<TResponse, TRequest>([NotNull] TRequest request)
            where TResponse : ResponseBase<TRequest>
            where TRequest : RequestBase;
    }
}