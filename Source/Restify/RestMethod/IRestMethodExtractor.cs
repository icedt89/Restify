namespace JanHafner.Restify.RestMethod
{
    using System.Net.Http;
    using JetBrains.Annotations;
    using Request;

    /// <summary>
    /// Provides methods for extracting the <see cref="HttpMethod"/> from the <see cref="RequestBase"/>.
    /// </summary>
    public interface IRestMethodExtractor
    {
        /// <summary>
        /// Extracts the <see cref="HttpMethod"/> from the <see cref="RequestBase"/>.
        /// </summary>
        /// <param name="request">The <see cref="RequestBase"/>.</param>
        /// <returns>The extracted <see cref="HttpMethod"/>.</returns>
        [NotNull]
        HttpMethod Extract([NotNull] RequestBase request);
    }
}