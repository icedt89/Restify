namespace JanHafner.Restify.Header
{
    using System;
    using Request;

    /// <summary>
    /// Returns all headers from the <see cref="RequestBase.HttpHeaders"/> property.
    /// </summary>
    public sealed class DefaultRequestHeaderCollector : IRequestHeaderCollector
    {
        /// <summary>
        /// Inspects the <see cref="RequestBase"/> and creates a <see cref="HttpHeaderCollection"/> with applicable headers.
        /// </summary>
        /// <param name="request">The <see cref="RequestBase"/>.</param>
        /// <param name="collectedHttpHeaders"></param>
        /// <returns>A new instance of the <see cref="HttpHeaderCollection"/> class.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="request"/>' and '<paramref name="collectedHttpHeaders"/>' cannot be null. </exception>
        public void CollectRequestHeaders(RequestBase request, HttpHeaderCollection collectedHttpHeaders)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            
            if (collectedHttpHeaders == null)
            {
                throw new ArgumentNullException(nameof(collectedHttpHeaders));
            }

            foreach (var requestHeader in request.HttpHeaders)
            {
                collectedHttpHeaders.Add(requestHeader);
            }
        }
    }
}