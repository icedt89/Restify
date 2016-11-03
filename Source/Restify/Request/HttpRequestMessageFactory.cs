namespace JanHafner.Restify.Request
{
    using System;
    using System.Net.Http;
    using Header;
    using JetBrains.Annotations;
    using Queryfy;
    using Queryfy.UrlPathBuilder;
    using Response;
    using RestMethod;

    /// <summary>
    /// Uses <see cref="IQueryfyDotNet"/>, <see cref="IUrlPathBuilder"/> and <see cref="IRestMethodExtractor"/> for building a <see cref="HttpRequestMessage"/>.
    /// </summary>
    public class HttpRequestMessageFactory : IHttpRequestMessageFactory
    {
        [NotNull]
        private readonly IQueryfyDotNet queryfyDotNet;

        [NotNull]
        private readonly IUrlPathBuilder urlPathBuilder;

        [NotNull]
        private readonly IRestMethodExtractor restMethodExtractor;

        [NotNull]
        private readonly IRequestHeaderFactory requestHeaderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestMessageFactory"/> class.
        /// </summary>
        /// <param name="queryfyDotNet">The <see cref="IQueryfyDotNet"/>.</param>
        /// <param name="urlPathBuilder">The <see cref="IUrlPathBuilder"/>.</param>
        /// <param name="restMethodExtractor">The <see cref="IRestMethodExtractor"/>.</param>
        /// <param name="requestHeaderFactory">The <see cref="IRequestHeaderFactory"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="queryfyDotNet"/>', '<paramref name="urlPathBuilder"/>', '<paramref name="restMethodExtractor"/>' and '<paramref name="requestHeaderFactory"/>' cannot be null. </exception>
        public HttpRequestMessageFactory([NotNull] IQueryfyDotNet queryfyDotNet, [NotNull] IUrlPathBuilder urlPathBuilder, [NotNull] IRestMethodExtractor restMethodExtractor, [NotNull] IRequestHeaderFactory requestHeaderFactory)
        {
            if (queryfyDotNet == null)
            {
                throw new ArgumentNullException(nameof(queryfyDotNet));
            }

            if (urlPathBuilder == null)
            {
                throw new ArgumentNullException(nameof(urlPathBuilder));
            }

            if (restMethodExtractor == null)
            {
                throw new ArgumentNullException(nameof(restMethodExtractor));
            }

            if (requestHeaderFactory == null)
            {
                throw new ArgumentNullException(nameof(requestHeaderFactory));
            }

            this.queryfyDotNet = queryfyDotNet;
            this.urlPathBuilder = urlPathBuilder;
            this.restMethodExtractor = restMethodExtractor;
            this.requestHeaderFactory = requestHeaderFactory;
        }

        /// <summary>
        /// Creates a new <see cref="HttpRequestMessage"/> from the supplied <see cref="RequestBase"/>.
        /// </summary>
        /// <param name="request">The <see cref="RequestBase"/>.</param>
        /// <returns>The created <see cref="HttpRequestMessage"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="request"/>' cannot be null. </exception>
        public HttpRequestMessage CreateHttpRequestMessage(RequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var queryPart = this.queryfyDotNet.Queryfy(request);
            var urlPart = this.urlPathBuilder.Build(request);
            var fullRequestUrl = urlPart + (String.IsNullOrWhiteSpace(queryPart.QueryString) ? String.Empty : "?" + queryPart.EncodedQueryString);
            var restMethod = this.restMethodExtractor.Extract(request);

            var result = new HttpRequestMessage(restMethod, fullRequestUrl);
            this.PrepareRequestBody(result, request);
            this.PrepareRequestHeaders(result, request);

            return result;
        }

        /// <summary>
        /// Prepares the request body.
        /// </summary>
        /// <param name="restRequest">The <see cref="HttpRequestMessage"/>.</param>
        /// <param name="request">The <see cref="ResponseBase"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="restRequest"/>' and '<paramref name="request"/>' cannot be null. </exception>
        protected virtual void PrepareRequestBody([NotNull] HttpRequestMessage restRequest, [NotNull] RequestBase request)
        {
            if (restRequest == null)
            {
                throw new ArgumentNullException(nameof(restRequest));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Get-Requests may not contain a body, otherwise an exception is thrown by the underlying infrastructure.
            if (restRequest.Method.Equals(HttpMethod.Get))
            {
                return;
            }

            request.PrepareRequestContent(restRequest);
        }

        /// <summary>
        /// Is called before the rest-request is send to the server.
        /// </summary>
        /// <param name="restRequest">The <see cref="HttpRequestMessage"/> to send.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="restRequest"/>' and '<paramref name="request"/>' cannot be null. </exception>
        /// <param name="request">The original abstracted <see cref="RequestBase"/> request.</param>
        protected virtual void PrepareRequestHeaders([NotNull] HttpRequestMessage restRequest, [NotNull] RequestBase request)
        {
            if (restRequest == null)
            {
                throw new ArgumentNullException(nameof(restRequest));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var collectedHeaders = this.requestHeaderFactory.CreateRequestHeaders(request);

            foreach (var requestHeader in collectedHeaders)
            {
                restRequest.Headers.Add(requestHeader.Name, requestHeader.Value);
            }
        }
    }
}