namespace JanHafner.Restify.Header
{
    using System;
    using System.Linq;
    using Request;
    using Toolkit.Common.ExtensionMethods;

    /// <summary>
    /// Returns all headers from the <see cref="FixedRequestHeaderAttribute"/>s of the <see cref="RequestBase"/>.
    /// </summary>
    public sealed class FixedRequestHeaderCollector : IRequestHeaderCollector
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

            foreach (var requestHeader in request.GetType().GetAttributesExactly<FixedRequestHeaderAttribute>().Select(fixedRequestHeaderAttribute => fixedRequestHeaderAttribute.ToHttpHeader()))
            {
                collectedHttpHeaders.Add(requestHeader);
            }
        }
    }
}