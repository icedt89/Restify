namespace JanHafner.Restify.Response
{
    using JetBrains.Annotations;
    using Request;

    /// <summary>
    /// Base class for all responses.
    /// </summary>
    public abstract class ResponseBase
    {
        /// <summary>
        /// The original request.
        /// </summary>
        [NotNull]
        // ReSharper disable once NotNullMemberIsNotInitialized Will always be initialized by the framework.
        public RequestBase OriginalRequest { get; internal set; }
    }

    /// <summary>
    /// Base class for all responses.
    /// </summary>
    /// <typeparam name="TRequest">The original <see cref="RequestBase"/>.</typeparam>
    public abstract class ResponseBase<TRequest> : ResponseBase
        where TRequest : RequestBase
    {
        /// <summary>
        /// The original request.
        /// </summary>
        [NotNull]
        new public TRequest OriginalRequest
        {
            get
            {
               return (TRequest)base.OriginalRequest;
            }
        }
    }
}