namespace JanHafner.Restify.Header
{
    using JetBrains.Annotations;
    using Request;

    /// <summary>
    /// Creates request headers from the supplied <see cref="RequestBase"/>.
    /// </summary>
    public interface IRequestHeaderFactory
    {
        /// <summary>
        /// Returns a new <see cref="HttpHeaderCollection"/> with collected headers from the <see cref="RequestBase"/>.
        /// </summary>
        /// <param name="request">The <see cref="RequestBase"/>.</param>
        /// <returns>The <see cref="HttpHeaderCollection"/>.</returns>
        [NotNull]
        HttpHeaderCollection CreateRequestHeaders([NotNull] RequestBase request);
    }
}