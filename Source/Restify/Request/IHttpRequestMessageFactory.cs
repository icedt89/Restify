namespace JanHafner.Restify.Request
{
    using System.Net.Http;
    using JetBrains.Annotations;

    /// <summary>
    /// Provides methods for creating <see cref="HttpRequestMessage"/> instances from a <see cref="RequestBase"/>.
    /// </summary>
    public interface IHttpRequestMessageFactory
    {
        /// <summary>
        /// Creates a new <see cref="HttpRequestMessage"/> from the supplied <see cref="RequestBase"/>.
        /// </summary>
        /// <param name="request">The <see cref="RequestBase"/>.</param>
        /// <returns>The created <see cref="HttpRequestMessage"/>.</returns>
        [NotNull]
        HttpRequestMessage CreateHttpRequestMessage([NotNull] RequestBase request);
    }
}