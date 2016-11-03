namespace JanHafner.Restify
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using Header;
    using JetBrains.Annotations;
    using Request;
    using Response;

    /// <summary>
    /// Processes requests via <see cref="HttpClient"/>.
    /// </summary>
    public class RequestProcessor : IRequestProcessor
    {
        /// <summary>
        /// The <see cref="HttpClient"/>.
        /// </summary>
        [NotNull]
        protected readonly HttpClient HttpClient;

        [NotNull]
        private readonly IHttpRequestMessageFactory httpRequestMessageFactory;

        [NotNull]
        private readonly IResponseHeaderMapper responseHeaderMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestProcessor"/> class.
        /// </summary>
        /// <param name="httpClient">An implementation of the <see cref="HttpClient"/> class.</param>
        /// <param name="httpRequestMessageFactory">The <see cref="IHttpRequestMessageFactory"/>.</param>
        /// <param name="responseHeaderMapper">The <see cref="IResponseHeaderMapper"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="httpClient"/>', '<paramref name="httpRequestMessageFactory"/>' and '<paramref name="responseHeaderMapper"/>' cannot be null. </exception>
        public RequestProcessor([NotNull] HttpClient httpClient, [NotNull] IHttpRequestMessageFactory httpRequestMessageFactory, [NotNull] IResponseHeaderMapper responseHeaderMapper)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if (httpRequestMessageFactory == null)
            {
                throw new ArgumentNullException(nameof(httpRequestMessageFactory));
            }

            if (responseHeaderMapper == null)
            {
                throw new ArgumentNullException(nameof(responseHeaderMapper));
            }

            this.HttpClient = httpClient;
            this.httpRequestMessageFactory = httpRequestMessageFactory;
            this.responseHeaderMapper = responseHeaderMapper;
        }

        /// <summary>
        /// Processes the request and returns the response.
        /// </summary>
        /// <typeparam name="TResponse">The result type of the response.</typeparam>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request to process.</param>
        /// <returns>The deserialized response.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="request"/>' cannot be null. </exception>
        public TResponse ProcessRequest<TResponse, TRequest>(TRequest request) 
            where TResponse : ResponseBase<TRequest> 
            where TRequest : RequestBase
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var httpRequestMessage = this.httpRequestMessageFactory.CreateHttpRequestMessage(request))
            {
                var httpResponseMessage = this.HttpClient.SendAsync(httpRequestMessage, CancellationToken.None).Result;

                this.ThrowException(httpResponseMessage);

                var deserializedResponse = request.CreateResponse(httpResponseMessage.Content.ReadAsStringAsync().Result);
                deserializedResponse.OriginalRequest = request;

                this.MapResponseHeaders(httpResponseMessage, deserializedResponse);

                return (TResponse)deserializedResponse;
            }
        }

        /// <summary>
        /// Maps the headers from the <see cref="HttpResponseMessage"/> to the response.
        /// </summary>
        /// <param name="httpResponseMessage">The <see cref="HttpResponseMessage."/></param>
        /// <param name="response">The response.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="httpResponseMessage"/>' and '<paramref name="response"/>' cannot be null. </exception>
        private void MapResponseHeaders([NotNull] HttpResponseMessage httpResponseMessage, [NotNull] ResponseBase response)
        {
            if (httpResponseMessage == null)
            {
                throw new ArgumentNullException(nameof(httpResponseMessage));
            }

            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var responseHeaderCollection = HttpHeaderCollection.FromHttpResponseMessage(httpResponseMessage);
            this.responseHeaderMapper.MapHeaders(response, responseHeaderCollection);
        }

        /// <summary>
        /// Throws a more concrete exception if the <see cref="HttpResponseMessage"/> does not contain useful detail.
        /// </summary>
        /// <param name="restResponse">The executed <see cref="HttpResponseMessage"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="restResponse"/>' cannot be null. </exception>
        protected virtual void ThrowException([NotNull] HttpResponseMessage restResponse)
        {
            if (restResponse == null)
            {
                throw new ArgumentNullException(nameof(restResponse));
            }

            try
            {
                restResponse.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException httpRequestException)
            {
                throw new HttpRequestExecutionException((Int32)restResponse.StatusCode, httpRequestException);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.HttpClient.Dispose();
        }
    }
}