namespace JanHafner.Restify.RestMethod
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;
    using Properties;
    using Request;

    /// <summary>
    /// Defines an exception which is thrown when no convention can be applied to extract the <see cref="HttpMethod"/> from the <see cref="RequestBase"/>.
    /// </summary>
    [Serializable]
    public sealed class RequestDoesNotFollowAnyImplementedConventionException : InvalidOperationException
    {
        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">The value of '<paramref name="conventions"/>' cannot be null. </exception>
        public RequestDoesNotFollowAnyImplementedConventionException([NotNull] RequestBase request, [NotNull] IEnumerable<Func<RequestBase, HttpMethod>> conventions) 
            : base(String.Format(ExceptionMessages.RequestDoesNotFollowAnyImplementedConventionExceptionMessage, request.GetType().Name))
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            this.Request = request;
            this.Conventions = conventions;
        }

        /// <inheritdoc />
        // ReSharper disable once NotNullMemberIsNotInitialized
        protected RequestDoesNotFollowAnyImplementedConventionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        
        /// <summary>
        /// Gets the <see cref="RequestBase"/>.
        /// </summary>
        [NotNull]
        public RequestBase Request { get; private set; }
        
        /// <summary>
        /// Gets a list of conventions.
        /// </summary>
        [NotNull]
        public IEnumerable<Func<RequestBase, HttpMethod>> Conventions { get; private set; }
    }
}
