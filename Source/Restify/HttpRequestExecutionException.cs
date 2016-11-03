namespace JanHafner.Restify
{
    using System;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;

    /// <summary>
    /// Adds a property which defines the status code of the response.
    /// </summary>
    [Serializable]
    public sealed class HttpRequestExecutionException : HttpRequestException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestExecutionException"/> class.
        /// </summary>
        /// <param name="httpStatusCode">The status code of the response.</param>
        /// <param name="httpRequestException">The wrapped <see cref="HttpRequestException"/>.</param>
        public HttpRequestExecutionException(Int32 httpStatusCode, [NotNull] HttpRequestException httpRequestException)
            : base(httpRequestException.Message, httpRequestException)
        {
            this.HttpStatusCode = httpStatusCode;
        }

        /// <inheritdoc />
        protected HttpRequestExecutionException(SerializationInfo info, StreamingContext context)
        {
        }

        /// <summary>
        /// The status code.
        /// </summary>
        public Int32 HttpStatusCode { get; private set; }
    }
}