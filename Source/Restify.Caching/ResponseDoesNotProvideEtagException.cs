namespace JanHafner.Restify.Caching
{
    using System;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;
    using Properties;
    using Response;

    /// <summary>
    /// Is thrown if the request does not provide an etag.
    /// </summary>
    [Serializable]
    public sealed class ResponseDoesNotProvideEtagException : ArgumentException
    {
        /// <inheritdoc />
        public ResponseDoesNotProvideEtagException([NotNull] ResponseBase response)
            : base(String.Format(ExceptionMessages.ResponseDoesNotProvideEtagExceptionMessage, response.GetType().Name))
        {
            this.Response = response;
        }

        /// <inheritdoc />
        public ResponseDoesNotProvideEtagException([NotNull] HttpResponseMessage httpResponseMessage)
            : base(String.Format(ExceptionMessages.ResponseDoesNotProvideEtagExceptionMessage, httpResponseMessage.RequestMessage.RequestUri))
        {
            this.HttpResponse = httpResponseMessage;
        }

        /// <inheritdoc />
        protected ResponseDoesNotProvideEtagException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// The response which not provides an ETag.
        /// </summary>
        [CanBeNull]
        public ResponseBase Response { get; set; }

        /// <summary>
        /// The http response which not provides an ETag.
        /// </summary>
        [CanBeNull]
        public HttpResponseMessage HttpResponse { get; set; }
    }
}
