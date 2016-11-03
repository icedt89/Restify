namespace JanHafner.Restify.Caching
{
    using System.Net.Http;
    using System.Net.Http.Headers;

    /// <summary>
    /// Just a class that sets the necessary properties for caching.
    /// </summary>
    public sealed class CachingAwareHttpClient : HttpClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachingAwareHttpClient"/> class.
        /// </summary>
        public CachingAwareHttpClient()
            : base(new ETagAwareWebRequestHandler())
        {
            this.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            this.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
        }
    }
}