namespace JanHafner.Restify.Response
{
    using Request;

    /// <summary>
    /// An empty response which contains no information.
    /// </summary>
    /// <typeparam name="TRequest">The original <see cref="RequestBase"/>.</typeparam>
    public sealed class EmptyResponse<TRequest> : ResponseBase<TRequest>
        where TRequest : RequestBase
    {
    }
}