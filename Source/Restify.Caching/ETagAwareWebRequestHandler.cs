namespace JanHafner.Restify.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Cache;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    /// <summary>
    /// Returns a cached copy of the <see cref="HttpResponseMessage"/> if the status code is NotModified.
    /// </summary>
    internal sealed class ETagAwareWebRequestHandler : WebRequestHandler
    {
        [NotNull]
        private readonly IDictionary<String, HttpResponseMessage> responseCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ETagAwareWebRequestHandler"/> class.
        /// </summary>
        public ETagAwareWebRequestHandler()
        {
            this.responseCache = new Dictionary<String, HttpResponseMessage>();
            this.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            this.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.CacheIfAvailable);
        }

        /// <summary>
        /// Creates an instance of  <see cref="T:System.Net.Http.HttpResponseMessage"/> based on the information provided in the <see cref="T:System.Net.Http.HttpRequestMessage"/> as an operation that will not block.
        /// </summary>
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1"/>.The task object representing the asynchronous operation.
        /// </returns>
        /// <param name="request">The HTTP request message.</param><param name="cancellationToken">A cancellation token to cancel the operation.</param><exception cref="T:System.ArgumentNullException">The <paramref name="request"/> was null.</exception>
        /// <exception cref="ResponseDoesNotProvideEtagException">The ETag is not present in the response.</exception>
        [NotNull]
        protected override async Task<HttpResponseMessage> SendAsync([NotNull] HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = await base.SendAsync(request, cancellationToken);

            var etag = result.ExtractETag();
            if (result.StatusCode == HttpStatusCode.NotModified && this.CachePolicy.Level == RequestCacheLevel.CacheIfAvailable)
            {
                if (String.IsNullOrWhiteSpace(etag))
                {
                    throw new ResponseDoesNotProvideEtagException(result);    
                }

                return this.responseCache[etag];
            }

            this.responseCache[etag] = result;

            result.StatusCode = HttpStatusCode.OK;
            return result;
        }
    }
}