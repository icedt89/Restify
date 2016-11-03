namespace JanHafner.Restify.Services
{
    using JetBrains.Annotations;
    using Request;
    using Response;

    /// <summary>
    /// Defines the base interface for all Rest services.
    /// </summary>
    public interface IRestService
    {
        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <typeparam name="TResponse">The expected response.</typeparam>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        [NotNull]
        TResponse ProcessRequest<TResponse, TRequest>([NotNull] TRequest request)
            where TResponse : ResponseBase<TRequest>
            where TRequest : RequestBase;
    }
}