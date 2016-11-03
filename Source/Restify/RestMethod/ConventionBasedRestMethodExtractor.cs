namespace JanHafner.Restify.RestMethod
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using JetBrains.Annotations;
    using Request;

    /// <summary>
    /// Extracts the <see cref="HttpMethod"/> from the type name.
    /// </summary>
    public sealed class ConventionBasedRestMethodExtractor : IRestMethodExtractor
    {
        [NotNull]
        private readonly IEnumerable<Func<RequestBase, HttpMethod>> conventions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionBasedRestMethodExtractor"/> class.
        /// </summary>
        public ConventionBasedRestMethodExtractor()
        {
            this.conventions = new List<Func<RequestBase, HttpMethod>>
                               {
                                   rb => rb.GetType().Name.StartsWith(HttpMethod.Get.ToString(), StringComparison.InvariantCultureIgnoreCase) ? HttpMethod.Get : null,
                                   rb => rb.GetType().Name.StartsWith(HttpMethod.Post.ToString(), StringComparison.InvariantCultureIgnoreCase) ? HttpMethod.Post : null,
                                   rb => rb.GetType().Name.StartsWith(HttpMethod.Put.ToString(), StringComparison.InvariantCultureIgnoreCase) ? HttpMethod.Put : null,
                                   rb => rb.GetType().Name.StartsWith(HttpMethod.Delete.ToString(), StringComparison.InvariantCultureIgnoreCase) ? HttpMethod.Delete : null,
                                   rb => rb.GetType().Name.StartsWith("Patch", StringComparison.InvariantCultureIgnoreCase) ? new HttpMethod("PATCH") : null,
                                   rb => rb.GetType().Name.StartsWith("GetHead", StringComparison.InvariantCultureIgnoreCase) ? HttpMethod.Head : null,
                                   rb => rb.GetType().Name.StartsWith(HttpMethod.Trace.ToString(), StringComparison.InvariantCultureIgnoreCase) ? HttpMethod.Trace : null,
                                   rb => rb.GetType().Name.StartsWith("GetOptions", StringComparison.InvariantCultureIgnoreCase) ? HttpMethod.Options : null,
                                   rb => rb.GetType().Name.StartsWith("Connect", StringComparison.InvariantCultureIgnoreCase) ? new HttpMethod("CONNECT") : null
                               };
        }

        /// <summary>
        /// Extracts the <see cref="HttpMethod"/> from the <see cref="RequestBase"/>.
        /// IF the type name of the request starts with "Get", "Post", "Put", "Delete" or "Patch" the corresponding <see cref="HttpMethod"/> will be returned.
        /// If the type name starts with "GetHead" or "GetOptions", "Head" or "Options" will be returned.
        /// </summary>
        /// <param name="request">The <see cref="RequestBase"/>.</param>
        /// <returns>The extracted <see cref="HttpMethod"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="request"/>' cannot be null. </exception>
        /// <exception cref="RequestDoesNotFollowAnyImplementedConventionException">The request '<paramref name="request"/>' does not follow any implemented conventions.</exception>
        public HttpMethod Extract(RequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            foreach (var convention in this.conventions)
            {
                var httpMethod = convention(request);
                if (httpMethod == null)
                {
                    continue;
                }

                return httpMethod;
            }

            throw new RequestDoesNotFollowAnyImplementedConventionException(request, this.conventions);
        }
    }
}