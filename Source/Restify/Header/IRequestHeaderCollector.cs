namespace JanHafner.Restify.Header
{
    using JetBrains.Annotations;
    using Request;

    /// <summary>
    /// Fills a <see cref="HttpHeaderCollection"/> out of a <see cref="RequestBase"/>.
    /// </summary>
    public interface IRequestHeaderCollector
    {
        /// <summary>
        /// Inspects the <see cref="RequestBase"/> and fill the <see cref="HttpHeaderCollection"/> with applicable headers.
        /// </summary>
        /// <param name="request">The <see cref="RequestBase"/>.</param>
        /// <param name="collectedHttpHeaders">The <see cref="HttpHeaderCollection"/> to fill with headers.</param>
        void CollectRequestHeaders([NotNull] RequestBase request, [NotNull] HttpHeaderCollection collectedHttpHeaders);
    }
}