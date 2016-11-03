namespace JanHafner.Restify.Services.RequestExecutionStrategy
{
    using JetBrains.Annotations;
    using Request;
    using Response;

    /// <summary>
    /// Defines the behavior that is applied for executing the request.
    /// </summary>
    public interface IRequestExecutionStrategy
    {
        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <typeparam name="TResponse">The result type of the response.</typeparam>
        /// <typeparam name="TRequest">The request.</typeparam>
        /// <param name="request">The request to process.</param>
        /// <returns>The deserialized response.</returns>
        [NotNull]
        TResponse Execute<TResponse, TRequest>([NotNull] TRequest request)
            where TResponse : ResponseBase<TRequest>
            where TRequest : RequestBase;
    }
}