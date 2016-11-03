namespace JanHafner.Restify.Request
{
    using System;
    using System.Net.Http;
    using Header;
    using JetBrains.Annotations;
    using Response;

    /// <summary>
    /// Base class for all requests.
    /// </summary>
    public abstract class RequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBase"/> class.
        /// </summary>
        protected RequestBase()
        {
            this.HttpHeaders = new HttpHeaderCollection();
        }

        /// <summary>
        /// Returns the content of this request.
        /// </summary>
        /// <returns>The content as object which gets serialized.</returns>
        protected virtual Object GetContent()
        {
            return this;
        }

        /// <summary>
        /// Sets headers and addition information regarding the body of the request.
        /// </summary>
        /// <param name="restRequest">The <see cref="HttpRequestMessage"/> to prepare.</param>
        public abstract void PrepareRequestContent([NotNull] HttpRequestMessage restRequest);

        /// <summary>
        /// Creates a derivation of the <see cref="ResponseBase"/> class for this request.
        /// </summary>
        /// <param name="restResponse">The response as <see cref="String"/>.</param>
        /// <returns>The response.</returns>
        [NotNull]
        public abstract ResponseBase CreateResponse([NotNull] String restResponse);

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        [NotNull]
        public HttpHeaderCollection HttpHeaders { get; private set; }
    }
}