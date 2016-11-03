namespace JanHafner.Restify.Header
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Request;

    /// <summary>
    /// Creates headers based on the merged result of all supplied <see cref="IRequestHeaderCollector"/>s.
    /// </summary>
    public sealed class RequestHeaderFactory : IRequestHeaderFactory
    {
        [NotNull]
        private readonly IEnumerable<IRequestHeaderCollector> requestHeaderCollectors;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHeaderFactory"/> class.
        /// </summary>
        /// <param name="requestHeaderCollectors">The <see cref="IRequestHeaderCollector"/>s.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="requestHeaderCollectors"/>' cannot be null. </exception>
        public RequestHeaderFactory([NotNull] IEnumerable<IRequestHeaderCollector> requestHeaderCollectors)
        {
            if (requestHeaderCollectors == null)
            {
                throw new ArgumentNullException(nameof(requestHeaderCollectors));    
            }

            // Enumerate the (maybe) injected enumerator from dependency injection.
            this.requestHeaderCollectors = requestHeaderCollectors.ToList();
        }

        /// <summary>
        /// Returns a new <see cref="HttpHeaderCollection"/> with collected headers from the <see cref="RequestBase"/>.
        /// </summary>
        /// <param name="request">The <see cref="RequestBase"/>.</param>
        /// <returns>The <see cref="HttpHeaderCollection"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="request"/>' cannot be null. </exception>
        public HttpHeaderCollection CreateRequestHeaders(RequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new HttpHeaderCollection();
            foreach (var requestHeaderCollector in this.requestHeaderCollectors)
            {
                requestHeaderCollector.CollectRequestHeaders(request, result);
            }

            return result;
        }
    }
}