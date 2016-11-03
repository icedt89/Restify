namespace JanHafner.Restify.RestMethod
{
    using System;
    using System.Net.Http;
    using JetBrains.Annotations;
    using Properties;

    /// <summary>
    /// Overrides the default behavior of the <see cref="RequestProcessor"/> class by providing an other Rest RestMethod.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RestMethodAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestMethodAttribute"/> class.
        /// </summary>
        /// <param name="restMethod">The Rest RestMethod.</param>
        public RestMethodAttribute(RestMethod restMethod)
        {
            this.RestMethod = restMethod;
        }

        /// <summary>
        /// The Rest RestMethod.
        /// </summary>
        public RestMethod RestMethod { get; private set; }

        /// <summary>
        /// Converts the <see cref="Restify.RestMethod.RestMethod"/> to the corresponding <see cref="HttpMethod"/>.
        /// </summary>
        /// <returns>The converted <see cref="HttpMethod"/>.</returns>
        [NotNull]
        public HttpMethod ToHttpMethod()
        {
            switch (this.RestMethod)
            {
                case RestMethod.Connect:
                    return new HttpMethod("CONNECT");
                case RestMethod.Delete:
                    return HttpMethod.Delete;
                case RestMethod.Get:
                    return HttpMethod.Get;
                case RestMethod.Head:
                    return HttpMethod.Head;
                case RestMethod.Options:
                    return HttpMethod.Options;
                case RestMethod.Patch:
                    return new HttpMethod("PATCH");
                case RestMethod.Post:
                    return HttpMethod.Post;
                case RestMethod.Put:
                    return HttpMethod.Put;
                case RestMethod.Trace:
                    return HttpMethod.Trace;
                default:
                    throw new InvalidOperationException(String.Format(ExceptionMessages.RestMethodNotConvertibleToHttpMethodExceptionMessage, this.RestMethod));
            }
        }
    }
}